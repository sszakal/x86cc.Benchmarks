using Docker.DotNet.Models;
using DotNet.Testcontainers;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Images;

namespace x86cc.Benchmarks.TestContainers.LavinMq;

/// <inheritdoc cref="ContainerBuilder" />
public sealed class LavinMqBuilder : ContainerBuilder<LavinMqBuilder, LavinMqContainer, LavinMqConfiguration>
{
    public const string LavinMqImage = "cloudamqp/lavinmq:latest";

    public const ushort LavinMqPort = 5672;

    public const string DefaultUsername = "lavinmq";

    /// <summary>
    /// Initializes a new instance of the <see cref="LavinMqBuilder" /> class.
    /// </summary>
    public LavinMqBuilder()
        : this(LavinMqImage)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LavinMqBuilder" /> class.
    /// </summary>
    /// <param name="image">
    /// The full Docker image name, including the image repository and tag
    /// </param>
    public LavinMqBuilder(string image)
        : this(new DockerImage(image))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LavinMqBuilder" /> class.
    /// </summary>
    /// <param name="image">
    /// An <see cref="IImage" /> instance that specifies the Docker image to be used
    /// for the container builder configuration.
    /// </param>
    public LavinMqBuilder(IImage image)
        : this(new LavinMqConfiguration())
    {
        DockerResourceConfiguration = Init().WithImage(image).DockerResourceConfiguration;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LavinMqBuilder" /> class.
    /// </summary>
    /// <param name="resourceConfiguration">The Docker resource configuration.</param>
    private LavinMqBuilder(LavinMqConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
        DockerResourceConfiguration = resourceConfiguration;
    }

    /// <inheritdoc />
    protected override LavinMqConfiguration DockerResourceConfiguration { get; }

    /// <summary>
    /// Sets the LavinMq username.
    /// </summary>
    /// <param name="username">The LavinMq username.</param>
    /// <returns>A configured instance of <see cref="LavinMqBuilder" />.</returns>
    public LavinMqBuilder WithUsername(string username)
    {
        return Merge(DockerResourceConfiguration, new LavinMqConfiguration(username: username))
            .WithEnvironment("LAVINMQ_DEFAULT_USER", username);
    }

    /// <summary>
    /// Sets the LavinMq password.
    /// </summary>
    /// <param name="password">The LavinMq password.</param>
    /// <returns>A configured instance of <see cref="LavinMqBuilder" />.</returns>
    private LavinMqBuilder WithPassword()
    {
        var lavinMqConfiguration = new LavinMqConfiguration();
        return Merge(DockerResourceConfiguration, lavinMqConfiguration)
            .WithEnvironment("LAVINMQ_DEFAULT_PASSWORD", lavinMqConfiguration.PasswordHash);
    }

    /// <inheritdoc />
    public override LavinMqContainer Build()
    {
        Validate();
        return new LavinMqContainer(DockerResourceConfiguration);
    }

    /// <inheritdoc />
    protected override LavinMqBuilder Init()
    {
        return base.Init()
            .WithPortBinding(LavinMqPort, true)
            .WithUsername(DefaultUsername)
            .WithPassword()
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("launcher Finished startup"));
    }

    /// <inheritdoc />
    protected override void Validate()
    {
        base.Validate();

        _ = Guard.Argument(DockerResourceConfiguration.Username, nameof(DockerResourceConfiguration.Username))
            .NotNull()
            .NotEmpty();

        _ = Guard.Argument(DockerResourceConfiguration.Password, nameof(DockerResourceConfiguration.Password))
            .NotNull()
            .NotEmpty();
    }

    /// <inheritdoc />
    protected override LavinMqBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new LavinMqConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override LavinMqBuilder Clone(IContainerConfiguration resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new LavinMqConfiguration(resourceConfiguration));
    }

    /// <inheritdoc />
    protected override LavinMqBuilder Merge(LavinMqConfiguration oldValue, LavinMqConfiguration newValue)
    {
        return new LavinMqBuilder(new LavinMqConfiguration(oldValue, newValue));
    }
}