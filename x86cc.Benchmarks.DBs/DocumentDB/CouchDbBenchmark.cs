using DotNet.Testcontainers.Containers;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Testcontainers.CouchDb;

namespace x86cc.Benchmarks.DBs.DocumentDB;

public class CouchDbBenchmark : DocumentDbBenchmark
{
    private const string DatabaseName = "benchmarks";
    private const string Username = "admin";
    private const string Password = "password";
    private const string IndexName = "createdAt-status";
    private const string IndexDesignDoc = "benchmarks-index";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly Dictionary<Guid, string> _revisionLookup = new();
    private HttpClient? _httpClient;

    protected override IContainer BuildContainer()
    {
        var builder = new CouchDbBuilder("couchdb:3.4.2")
            .WithUsername(Username)
            .WithPassword(Password);

        var platform = ResolveDockerPlatform();
        if (!string.IsNullOrWhiteSpace(platform))
        {
            builder = builder.WithCreateParameterModifier(p => p.Platform = platform);
        }

        return builder.Build();
    }

    private string GetConnectionString(IContainer container)
    {
        return ((CouchDbContainer)container).GetConnectionString();
    }

    protected override async Task InitializeAsync(IContainer container)
    {
        var connectionString = GetConnectionString(container);
        _httpClient = new HttpClient { BaseAddress = new Uri(AppendTrailingSlash(connectionString)) };
        var authBytes = Encoding.ASCII.GetBytes($"{Username}:{Password}");
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));

        await EnsureCouchReadyAsync().ConfigureAwait(false);
        await EnsureDatabaseAsync().ConfigureAwait(false);
        await CreateIndexesAsync().ConfigureAwait(false);
    }

    protected override async Task ResetDatabaseAsync()
    {
        await DeleteDatabaseAsync().ConfigureAwait(false);
        await EnsureDatabaseAsync().ConfigureAwait(false);
        await CreateIndexesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteInsertAsync(CustomerOrderRoot item)
    {
        var document = ToDocument(item);
        var result = await PutDocumentAsync(document).ConfigureAwait(false);
        _revisionLookup[item.Id] = result.Rev;
    }

    protected override async Task ExecuteInsertBulkAsync(CustomerOrderRoot[] items)
    {
        var documents = items.Select(item => ToDocument(item)).ToArray();
        var results = await SendBulkDocsAsync(documents).ConfigureAwait(false);

        foreach (var result in results)
        {
            if (!string.IsNullOrWhiteSpace(result.Error))
            {
                throw new InvalidOperationException($"CouchDB bulk insert failed: {result.Error} - {result.Reason}");
            }

            _revisionLookup[FromDocumentId(result.Id)] = result.Rev;
        }
    }

    protected override async Task ExecuteUpdateAsync(CustomerOrderRoot order)
    {
        var revision = _revisionLookup[order.Id];
        var document = ToDocument(order, revision);
        var result = await PutDocumentAsync(document).ConfigureAwait(false);
        _revisionLookup[order.Id] = result.Rev;
    }

    protected override async Task ExecuteUpdateBulkAsync(CustomerOrderRoot[] orders)
    {
        var documents = orders
            .Select(order => ToDocument(order, _revisionLookup[order.Id]))
            .ToArray();

        var results = await SendBulkDocsAsync(documents).ConfigureAwait(false);

        foreach (var result in results)
        {
            if (!string.IsNullOrWhiteSpace(result.Error))
            {
                throw new InvalidOperationException($"CouchDB bulk update failed: {result.Error} - {result.Reason}");
            }

            _revisionLookup[FromDocumentId(result.Id)] = result.Rev;
        }
    }

    protected override async Task ExecuteSearchAsync(Expression<Func<CustomerOrderRoot, bool>> filter, int take)
    {
        var query = new CouchDbFindRequest
        {
            Selector = new Dictionary<string, object>
            {
                ["status"] = (int)OrderStatus.Submitted,
                ["createdAt"] = new Dictionary<string, object>
                {
                    ["$gt"] = DateTime.UnixEpoch.ToString("O")
                }
            },
            Limit = take,
            Sort =
            [
                new Dictionary<string, string> { ["createdAt"] = "asc" }
            ],
            UseIndex = [ IndexDesignDoc, IndexName ]
        };

        var payload = JsonSerializer.Serialize(query, JsonOptions);
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");
        using var response = await _httpClient!.PostAsync($"{DatabaseName}/_find", content).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            throw new InvalidOperationException(
                $"CouchDB _find failed with {(int)response.StatusCode} {response.ReasonPhrase}: {errorBody}");
        }
        var results = await ReadResponseAsync<CouchDbFindResult>(response).ConfigureAwait(false);
        
        if (results.Docs.Length != 0) return;
        throw new Exception("No results found");
    }

    protected override async Task<CustomerOrderRoot?> ExecuteLoadByIdAsync(Guid id)
    {
        var documentId = ToDocumentId(id);
        using var response = await _httpClient!.GetAsync($"{DatabaseName}/{Uri.EscapeDataString(documentId)}")
            .ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        var document = await ReadResponseAsync<CouchDbOrderDocument>(response).ConfigureAwait(false);
        return document;
    }

    protected override async Task ExecuteDeleteAsync(CustomerOrderRoot order)
    {
        if (!_revisionLookup.TryGetValue(order.Id, out var revision))
        {
            return;
        }

        var documentId = ToDocumentId(order.Id);
        using var response = await _httpClient!.DeleteAsync(
            $"{DatabaseName}/{Uri.EscapeDataString(documentId)}?rev={Uri.EscapeDataString(revision)}")
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        _revisionLookup.Remove(order.Id);
    }

    protected override async Task DeleteManyAsync(CustomerOrderRoot[] orders)
    {
        var documents = orders
            .Where(order => _revisionLookup.ContainsKey(order.Id))
            .Select(order => ToDocument(order, _revisionLookup[order.Id], deleted: true))
            .ToArray();

        if (documents.Length == 0)
        {
            return;
        }

        var results = await SendBulkDocsAsync(documents).ConfigureAwait(false);

        foreach (var result in results)
        {
            if (!string.IsNullOrWhiteSpace(result.Error))
            {
                throw new InvalidOperationException($"CouchDB bulk delete failed: {result.Error} - {result.Reason}");
            }

            _revisionLookup.Remove(FromDocumentId(result.Id));
        }
    }

    private async Task EnsureCouchReadyAsync()
    {
        var deadline = DateTime.UtcNow.AddSeconds(30);
        while (true)
        {
            try
            {
                using var response = await _httpClient!.GetAsync("_up").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch (HttpRequestException) when (DateTime.UtcNow < deadline)
            {
            }

            if (DateTime.UtcNow >= deadline)
            {
                throw new TimeoutException("CouchDB did not become ready in time.");
            }

            await Task.Delay(500).ConfigureAwait(false);
        }
    }

    private async Task EnsureDatabaseAsync()
    {
        using var response = await _httpClient!.PutAsync(DatabaseName, content: null).ConfigureAwait(false);
        if (response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.PreconditionFailed)
        {
            response.EnsureSuccessStatusCode();
        }
    }

    private async Task DeleteDatabaseAsync()
    {
        using var response = await _httpClient!.DeleteAsync(DatabaseName).ConfigureAwait(false);
        if (response.StatusCode != HttpStatusCode.NotFound)
        {
            response.EnsureSuccessStatusCode();
        }
    }

    private async Task CreateIndexesAsync()
    {
        var indexRequest = new
        {
            index = new { fields = new[] { "createdAt", "status" } },
            name = IndexName,
            ddoc = IndexDesignDoc,
            type = "json"
        };

        var payload = JsonSerializer.Serialize(indexRequest, JsonOptions);
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");
        using var response = await _httpClient!.PostAsync($"{DatabaseName}/_index", content).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    private async Task<CouchDbWriteResult> PutDocumentAsync(CouchDbOrderDocument document)
    {
        var payload = JsonSerializer.Serialize(document, JsonOptions);
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");
        using var response = await _httpClient!.PutAsync(
                $"{DatabaseName}/{Uri.EscapeDataString(document.CouchId!)}",
                content)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await ReadResponseAsync<CouchDbWriteResult>(response).ConfigureAwait(false);
    }

    private async Task<IReadOnlyList<CouchDbWriteResult>> SendBulkDocsAsync(CouchDbOrderDocument[] documents)
    {
        var request = new CouchDbBulkRequest { Docs = documents };
        var payload = JsonSerializer.Serialize(request, JsonOptions);
        using var content = new StringContent(payload, Encoding.UTF8, "application/json");
        using var response = await _httpClient!.PostAsync($"{DatabaseName}/_bulk_docs", content).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await ReadResponseAsync<IReadOnlyList<CouchDbWriteResult>>(response).ConfigureAwait(false);
    }

    private static async Task<T> ReadResponseAsync<T>(HttpResponseMessage response)
    {
        await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        var result = await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions).ConfigureAwait(false);
        return result ?? throw new InvalidOperationException("CouchDB response deserialized to null.");
    }

    private static CouchDbOrderDocument ToDocument(CustomerOrderRoot order, string? revision = null, bool deleted = false)
    {
        return new CouchDbOrderDocument
        {
            CouchId = ToDocumentId(order.Id),
            Revision = revision,
            Deleted = deleted ? true : null,
            Id = order.Id,
            OrderNumber = order.OrderNumber,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Customer = order.Customer,
            ShippingAddress = order.ShippingAddress,
            BillingAddress = order.BillingAddress,
            Payment = order.Payment,
            Lines = order.Lines,
            Discounts = order.Discounts,
            Audit = order.Audit,
            Metadata = order.Metadata
        };
    }

    private static string ToDocumentId(Guid id)
    {
        return $"orders-{id:N}";
    }

    private static Guid FromDocumentId(string id)
    {
        if (id.StartsWith("orders-", StringComparison.OrdinalIgnoreCase) &&
            Guid.TryParseExact(id.AsSpan("orders-".Length), "N", out var guid))
        {
            return guid;
        }

        return Guid.Parse(id);
    }

    private static string AppendTrailingSlash(string value)
    {
        return value.EndsWith('/') ? value : $"{value}/";
    }

    private sealed class CouchDbOrderDocument : CustomerOrderRoot
    {
        [JsonPropertyName("_id")]
        public string? CouchId { get; set; }

        [JsonPropertyName("_rev")]
        public string? Revision { get; set; }

        [JsonPropertyName("_deleted")]
        public bool? Deleted { get; set; }
    }

    private sealed class CouchDbWriteResult
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("rev")]
        public string Rev { get; set; } = string.Empty;

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
    }

    private sealed class CouchDbBulkRequest
    {
        [JsonPropertyName("docs")]
        public CouchDbOrderDocument[] Docs { get; set; } = [];
    }

    private sealed class CouchDbFindRequest
    {
        [JsonPropertyName("selector")]
        public Dictionary<string, object> Selector { get; set; } = [];

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("sort")]
        public List<Dictionary<string, string>> Sort { get; set; } = [];

        [JsonPropertyName("use_index")]
        public string[]? UseIndex { get; set; }
    }

    private sealed class CouchDbFindResult
    {
        [JsonPropertyName("docs")]
        public CouchDbOrderDocument[] Docs { get; set; } = [];
    }
}
