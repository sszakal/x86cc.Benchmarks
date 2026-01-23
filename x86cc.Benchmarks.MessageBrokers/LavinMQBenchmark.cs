using System.Diagnostics;
using System.Text;
using BenchmarkDotNet.Attributes;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using x86cc.Benchmarks.TestContainers.LavinMq;

namespace x86cc.Benchmarks.MessageBrokers;

public class LavinMQBenchmark: MessageBrokerBenchmark, IAsyncDisposable
{
    private const string BenchmarkQueue = "benchmark_queue";
    private readonly LavinMqContainer _container = new LavinMqBuilder().WithImage(new DockerImage("cloudamqp/lavinmq:latest")).Build();
    private IConnection? _connection;
    private IChannel? _channel;
    private AsyncEventingBasicConsumer? _consumer;

    protected override async Task Initialise()
    {
        if(_container.State != TestcontainersStates.Running)
            await _container.StartAsync().ConfigureAwait(false);

        var channelOpts = new CreateChannelOptions(
            publisherConfirmationsEnabled: PublisherConfirmationsEnabled,
            publisherConfirmationTrackingEnabled: publisherConfirmationTrackingEnabled
        );

        (_connection, _channel) = await SetupConnection(channelOpts, _container.GetConnectionString());
        
        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.ReceivedAsync += async (sender, deliveryEventArgs) =>
        {
            if(_tcs == null) return;
            var body = deliveryEventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var receiveTime = Stopwatch.GetTimestamp();
            _tcs.TrySetResult(receiveTime);
        };
        
        string consumerTag = await _channel.BasicConsumeAsync(BenchmarkQueue, true, _consumer);
    }

    private async Task<(IConnection, IChannel)> SetupConnection(CreateChannelOptions channelOpts, string connectionString)
    {
        var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync(channelOpts);
        await channel.QueueDeclareAsync(BenchmarkQueue, durable: DurableQueue, exclusive: false, autoDelete: false);
        return (connection, channel);
    }
    
    protected override ValueTask Publish(ReadOnlyMemory<byte> message)
    {
        return _channel!.BasicPublishAsync( string.Empty, BenchmarkQueue, true, message);
    }
    
    [GlobalCleanup]
    public async Task GlobalCleanup()
    {
        await _channel!.DisposeAsync();
        await _connection!.DisposeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}