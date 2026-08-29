using Docker.DotNet.Models;
using DotNet.Testcontainers;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Images;

namespace x86cc.Benchmarks.AspNetCore.Containers.LavinMq;

public sealed class LavinMqBuilder : ContainerBuilder<LavinMqBuilder, LavinMqContainer, LavinMqConfiguration>
{
    public const string LavinMqImage = "cloudamqp/lavinmq:latest";
    public const ushort LavinMqPort = 5672;
    public const string DefaultUsername = "lavinmq";

    public LavinMqBuilder()
        : this(LavinMqImage)
    {
    }

    public LavinMqBuilder(string image)
        : this(new DockerImage(image))
    {
    }

    public LavinMqBuilder(IImage image)
        : this(new LavinMqConfiguration())
    {
        DockerResourceConfiguration = Init().WithImage(image).DockerResourceConfiguration;
    }

    private LavinMqBuilder(LavinMqConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
        DockerResourceConfiguration = resourceConfiguration;
    }

    protected override LavinMqConfiguration DockerResourceConfiguration { get; }

    public LavinMqBuilder WithUsername(string username)
    {
        return Merge(DockerResourceConfiguration, new LavinMqConfiguration(username: username))
            .WithEnvironment("LAVINMQ_DEFAULT_USER", username);
    }

    private LavinMqBuilder WithPassword()
    {
        var lavinMqConfiguration = new LavinMqConfiguration();
        return Merge(DockerResourceConfiguration, lavinMqConfiguration)
            .WithEnvironment("LAVINMQ_DEFAULT_PASSWORD", lavinMqConfiguration.PasswordHash);
    }

    public override LavinMqContainer Build()
    {
        Validate();
        return new LavinMqContainer(DockerResourceConfiguration);
    }

    protected override LavinMqBuilder Init()
    {
        return base.Init()
            .WithPortBinding(LavinMqPort, true)
            .WithUsername(DefaultUsername)
            .WithPassword()
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("launcher Finished startup"));
    }

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

    protected override LavinMqBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new LavinMqConfiguration(resourceConfiguration));
    }

    protected override LavinMqBuilder Clone(IContainerConfiguration resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new LavinMqConfiguration(resourceConfiguration));
    }

    protected override LavinMqBuilder Merge(LavinMqConfiguration oldValue, LavinMqConfiguration newValue)
    {
        return new LavinMqBuilder(new LavinMqConfiguration(oldValue, newValue));
    }
}
