using DotNetTestNSpec.Domain.VisualStudio;
using Microsoft.Extensions.Testing.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNetTestNSpec.Domain.DesignTime
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
            SendMessage(new Message()
            {
                MessageType = testFoundMessageType,
                Payload = JToken.FromObject(test),
            });
        }

        public void Disconnect()
        {
            SendMessage(new Message()
            {
                MessageType = testCompletedMessageType,
            });

            channel.Close();
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
