using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace x86cc.Benchmarks.AspNetCore.Containers.LavinMq;

public sealed class LavinMqConfiguration : ContainerConfiguration
{
    public LavinMqConfiguration(string? username = null)
    {
        Username = username;
    }

    public LavinMqConfiguration(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
        : base(resourceConfiguration)
    {
    }

    public LavinMqConfiguration(IContainerConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
    }

    public LavinMqConfiguration(LavinMqConfiguration resourceConfiguration)
        : this(new LavinMqConfiguration(), resourceConfiguration)
    {
    }

    public LavinMqConfiguration(LavinMqConfiguration oldValue, LavinMqConfiguration newValue)
        : base(oldValue, newValue)
    {
        Username = BuildConfiguration.Combine(oldValue.Username, newValue.Username);
    }

    public string? Username { get; }

    public string? Password => "guest";
    public string? PasswordHash => "+pHuxkR9fCyrrwXjOD4BP4XbzO3l8LJr8YkThMgJ0yVHFRE+";
}
