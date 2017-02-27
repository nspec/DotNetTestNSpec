using DotNetTestNSpec.Dev.IO.VisualStudio;
using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Domain.VisualStudio;

namespace DotNetTestNSpec.IO.VisualStudio
{
    public class ChannelFactory : IChannelFactory
    {
        public ChannelFactory(LaunchOptions options)
        {
            this.options = options;
        }

        public INetworkChannel Create()
        {
            var channel = options.NSpec.DebugChannel
                ? new ConsoleDebugChannel(options.NSpec.DebugTests) as INetworkChannel
                : new NetworkChannel(options.DotNet.Port.Value);

            return channel;
        }

        readonly LaunchOptions options;
    }
}
