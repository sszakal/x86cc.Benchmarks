using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using x86cc.Benchmarks.Common;

namespace x86cc.Benchmarks.MessageBrokers;

[BenchmarkCategory("MessageBrokers")]
[ExceptionDiagnoser]
[GcServer(true)]
[SimpleJob(RuntimeMoniker.Net10_0, warmupCount: 1, iterationCount: 5)]
public abstract class MessageBrokerBenchmark
{
    private ReadOnlyMemory<byte> _messageBody;
    private Memory<byte>[]? _messages;
    protected TaskCompletionSource<long>? _tcs;
    
    [Params(100000)]
    public int MessageCount { get; set; }
    
    [Params(8192)]
    public int MessageSize { get; set; }
    
    [Params(true)]
    public bool DurableQueue { get; set; } 
    
    [Params(true)]
    public bool PublisherConfirmationsEnabled { get; set; }    
    
    [Params(true)]
    public bool PublisherConfirmationTrackingEnabled { get; set; }

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        _messageBody = Encoding.UTF8.GetBytes(RandomStringGenerator.GenerateRandomString(MessageSize)).AsMemory();
        _messages = Enumerable.Range(0, MessageCount).Select(i => Encoding.UTF8.GetBytes(RandomStringGenerator.GenerateRandomString(MessageSize)).AsMemory()).ToArray();
        await Initialise();
    }

    [Benchmark]
    public async Task OneMessageAtATime()
    {
        for (int i = 0; i < MessageCount; i++)
        {
            await Publish(_messageBody);
        }
    }
    
    [Benchmark]
    public async Task BulkPublish()
    {
        var publishTasks = new List<ValueTask>();
        for (int i = 0; i < MessageCount; i++)
        {
            ValueTask publishTask = Publish(_messages![i]);
            publishTasks.Add(publishTask);

            await MaybeAwaitPublishes(publishTasks, MessageCount);
        }

        // Await any remaining tasks in case message count was not
        // evenly divisible by batch size.
        await MaybeAwaitPublishes(publishTasks, 0);
    }
    
    [Benchmark]
    public async Task MessageLatency()
    {
        for (int i = 0; i < MessageCount; i++)
        {
            _tcs = new TaskCompletionSource<long>();
            await Publish(_messageBody);
            await _tcs.Task.ConfigureAwait(false);
        }
    }
    
    static async Task MaybeAwaitPublishes(List<ValueTask> publishTasks, int batchSize)
    {
        if (publishTasks.Count >= batchSize)
        {
            foreach (var pt in publishTasks)
            {
                try
                {
                    await pt;
                }
                catch (Exception ex)
                {
                    await Console.Error.WriteLineAsync($"{DateTime.Now} [ERROR] saw nack or return, ex: '{ex}'");
                }
            }
            publishTasks.Clear();
        }
    }

    protected abstract Task Initialise();
    
    protected abstract ValueTask Publish(ReadOnlyMemory<byte> message);
}