using DotNetTestNSpec.Domain.VisualStudio;
using Microsoft.Extensions.Testing.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNetTestNSpec.Domain.DesignTime
{
    public class DiscoveryAdapter : IDiscoveryAdapter
    {
        public DiscoveryAdapter(IChannelFactory channelFactory)
        {
            this.channelFactory = channelFactory;
        }

        public IDiscoveryConnection Connect()
        {
            var channel = channelFactory.Create();

            return new Connection(channel);
        }

        readonly IChannelFactory channelFactory;

        public class Connection : IDiscoveryConnection
        {
            public Connection(INetworkChannel channel)
            {
                this.channel = channel;

                channel.Open();
            }

            public void Dispose()
            {
                SendMessage(new Message()
                {
                    MessageType = testCompletedMessageType,
                });

                channel.Close();
            }

            public void TestFound(Test test)
            {
                SendMessage(new Message()
                {
                    MessageType = testFoundMessageType,
                    Payload = JToken.FromObject(test),
                });
            }

            void SendMessage(Message message)
            {
                string serialized = JsonConvert.SerializeObject(message);

                channel.Send(serialized);
            }

            readonly INetworkChannel channel;

            const string testFoundMessageType = "TestDiscovery.TestFound";
            const string testCompletedMessageType = "TestRunner.TestCompleted";
        }
    }
}
