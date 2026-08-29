using DotNet.Testcontainers.Containers;

namespace x86cc.Benchmarks.AspNetCore.Containers.LavinMq;

public sealed class LavinMqContainer : DockerContainer
{
    private readonly LavinMqConfiguration _configuration;

    public LavinMqContainer(LavinMqConfiguration configuration)
        : base(configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString()
    {
        var endpoint = new UriBuilder("amqp", Hostname, GetMappedPublicPort(LavinMqBuilder.LavinMqPort));
        if (_configuration.Username != null) endpoint.UserName = Uri.EscapeDataString(_configuration.Username);
        if (_configuration.Password != null) endpoint.Password = Uri.EscapeDataString(_configuration.Password);
        return endpoint.ToString();
    }
}
