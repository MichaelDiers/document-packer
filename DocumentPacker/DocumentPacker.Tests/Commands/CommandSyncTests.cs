namespace DocumentPacker.Tests.Commands;

using DocumentPacker.Commands;
using Libs.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Tests for <see cref="ICommandSync" />.
/// </summary>
public class CommandSyncTests
{
    private readonly ICommandSync commandSync = CustomServiceProviderBuilder
        .Build(CommandsServiceCollectionExtensions.TryAddCommands)
        .GetRequiredService<ICommandSync>();

    [Fact]
    public void AfterExit_EnterShouldSucceed()
    {
        Assert.True(this.commandSync.Enter());
        Assert.False(this.commandSync.Enter());

        this.commandSync.Exit();

        Assert.True(this.commandSync.Enter());
    }

    [Fact]
    public void Enter_ShouldFail_IfCommandIsRunning()
    {
        Assert.True(this.commandSync.Enter());
        Assert.False(this.commandSync.Enter());
    }

    [Fact]
    public void Enter_ShouldFail_IfCommandIsRunningAndForceIsFalse()
    {
        Assert.True(this.commandSync.Enter());
        Assert.False(this.commandSync.Enter());
    }

    [Fact]
    public void Enter_ShouldSucceed_IfCommandIsRunningAndForceIsTrue()
    {
        Assert.True(this.commandSync.Enter());
        Assert.True(this.commandSync.Enter(true));
    }

    [Fact]
    public void Enter_ShouldSucceed_IfNoCommandIsRunning()
    {
        Assert.True(this.commandSync.Enter());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Enter_ShouldSucceed_IfNoCommandIsRunningIndependentFromForce(bool force)
    {
        Assert.True(this.commandSync.Enter(force));
    }

    [Fact]
    public void IsCommandActive()
    {
        Assert.False(this.commandSync.IsCommandActive);

        this.commandSync.Enter();

        Assert.True(this.commandSync.IsCommandActive);
    }
}
