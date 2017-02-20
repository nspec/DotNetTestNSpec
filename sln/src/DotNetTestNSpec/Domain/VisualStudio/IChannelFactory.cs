namespace DotNetTestNSpec.Domain.VisualStudio
{
    public interface IChannelFactory
    {
        INetworkChannel Create(LaunchOptions options);
    }
}
