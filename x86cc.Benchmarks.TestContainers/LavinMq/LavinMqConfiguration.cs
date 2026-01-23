using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace x86cc.Benchmarks.TestContainers.LavinMq;

/// <inheritdoc cref="ContainerConfiguration" />
public sealed class LavinMqConfiguration : ContainerConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LavinMqConfiguration" /> class.
    /// </summary>
    /// <param name="username">The RabbitMq username.</param>
    /// <param name="password">The RabbitMq password.</param>
    public LavinMqConfiguration(
        string? username = null)
    {
        Username = username;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LavinMqConfiguration" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    public LavinMqConfiguration(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
        : base(resourceConfiguration)
    {
        // Passes the configuration upwards to the base implementations to create an updated immutable copy.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LavinMqConfiguration" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    public LavinMqConfiguration(IContainerConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
        // Passes the configuration upwards to the base implementations to create an updated immutable copy.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LavinMqConfiguration" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    public LavinMqConfiguration(LavinMqConfiguration resourceConfiguration)
        : this(new LavinMqConfiguration(), resourceConfiguration)
    {
        // Passes the configuration upwards to the base implementations to create an updated immutable copy.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LavinMqConfiguration" /> class.
    /// </summary>
    /// <param name="oldValue">The old Docker resource configuration.</param>
    /// <param name="newValue">The new Docker resource configuration.</param>
    public LavinMqConfiguration(LavinMqConfiguration oldValue, LavinMqConfiguration newValue)
        : base(oldValue, newValue)
    {
        Username = BuildConfiguration.Combine(oldValue.Username, newValue.Username);
    }

    /// <summary>
    /// Gets the RabbitMq username.
    /// </summary>
    public string? Username { get; }

    /// <summary>
    /// Gets the LavinMq password.
    /// </summary>
    public string? Password => "guest";
    public string? PasswordHash => "+pHuxkR9fCyrrwXjOD4BP4XbzO3l8LJr8YkThMgJ0yVHFRE+";
}