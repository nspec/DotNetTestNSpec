using DotNetTestNSpec.Dev.IO.VisualStudio;
using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Domain.VisualStudio;

namespace DotNetTestNSpec.IO.VisualStudio
{
    public class ChannelFactory : IChannelFactory
    {
        public INetworkChannel Create(LaunchOptions options)
        {
            var channel = options.NSpec.DebugChannel
                ? new ConsoleDebugChannel(options.NSpec.DebugTests) as INetworkChannel
                : new NetworkChannel(options.DotNet.Port.Value);

            return channel;
        }
    }
}
