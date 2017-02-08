using Microsoft.Extensions.Testing.Abstractions;
using DotNetTestNSpec.Network;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace DotNetTestNSpec.DesignTime
{
    public class DiscoveryAdapter : IDiscoveryAdapter
    {
        public DiscoveryAdapter(INetworkChannel channel)
        {
            this.channel = channel;
        }

        public void Connect()
        {
            channel.Open();
        }

        public void TestFound(Test test)
        {
            var message = new Message()
            {
                MessageType = testFoundMessageType,
                Payload = JToken.FromObject(test),
            };

            string serialized = JsonConvert.SerializeObject(message);

            channel.Send(serialized);
        }

        public void Disconnect()
        {
            var message = new Message()
            {
                MessageType = testCompletedMessageType,
            };

            string serialized = JsonConvert.SerializeObject(message);

            channel.Send(serialized);

            channel.Close();
        }

        readonly INetworkChannel channel;

        const string testFoundMessageType = "TestDiscovery.TestFound";
        const string testCompletedMessageType = "TestRunner.TestCompleted";
    }
}
