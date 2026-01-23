using DotNet.Testcontainers.Containers;

namespace x86cc.Benchmarks.TestContainers.LavinMq;

public sealed class LavinMqContainer : DockerContainer
{
    private readonly LavinMqConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="LavinMqContainer" /> class.
    /// </summary>
    /// <param name="configuration">The container configuration.</param>
    public LavinMqContainer(LavinMqConfiguration configuration)
        : base(configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Gets the LavinMq connection string.
    /// </summary>
    /// <returns>The LavinMq connection string.</returns>
    public string GetConnectionString()
    {
        var endpoint = new UriBuilder("amqp", Hostname, GetMappedPublicPort(LavinMqBuilder.LavinMqPort));
        if (_configuration.Username != null) endpoint.UserName = Uri.EscapeDataString(_configuration.Username);
        if (_configuration.Password != null) endpoint.Password = Uri.EscapeDataString(_configuration.Password);
        return endpoint.ToString();
    }
}