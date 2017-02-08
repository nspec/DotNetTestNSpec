using DotNetTestNSpec.DesignTime;
using DotNetTestNSpec.Network;
using FluentAssertions;
using Microsoft.Extensions.Testing.Abstractions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DotNetTestNSpec.Tests.DesignTime
{
    [TestFixture]
    [Category("DiscoveryAdapter")]
    public abstract class describe_DiscoveryAdapter
    {
        protected DiscoveryAdapter adapter;

        protected Mock<INetworkChannel> channel;
        protected List<string> sentMessages;

        [SetUp]
        public virtual void setup()
        {
            channel = new Mock<INetworkChannel>();

            channel.Setup(ch => ch.Send(It.IsAny<string>())).Callback((string message) =>
            {
                sentMessages.Add(message);
            });

            sentMessages = new List<string>();

            adapter = new DiscoveryAdapter(channel.Object);
        }
    }

    public class when_connected : describe_DiscoveryAdapter
    {
        public override void setup()
        {
            base.setup();

            adapter.Connect();
        }

        [TestCase]
        public void it_should_open_channel()
        {
            channel.Verify(ch => ch.Open(), Times.Once);
        }
    }

    public class when_test_found : describe_DiscoveryAdapter
    {
        readonly Test someTest = new Test()
        {
            FullyQualifiedName = "nspec. A class. A context. Some Example",
            DisplayName = "A class › A context › Some Example",
            CodeFilePath = @"some\path\to\code",
            LineNumber = 10,
        };

        public override void setup()
        {
            base.setup();

            adapter.TestFound(someTest);
        }

        [TestCase]
        public void it_should_send_test_found_payload()
        {
            string expected = JsonConvert.SerializeObject(new Message()
            {
                MessageType = "TestDiscovery.TestFound",
                Payload = JToken.FromObject(someTest),
            });

            sentMessages.ShouldBeEquivalentTo(new[] { expected });
        }
    }

    public class when_disconnected : describe_DiscoveryAdapter
    {
        public override void setup()
        {
            base.setup();

            adapter.Disconnect();
        }

        [TestCase]
        public void it_should_send_test_completed_payload()
        {
            string expected = JsonConvert.SerializeObject(new Message()
            {
                MessageType = "TestRunner.TestCompleted",
                Payload = null,
            });

            sentMessages.ShouldBeEquivalentTo(new[] { expected });
        }

        [TestCase]
        public void it_should_close_channel()
        {
            channel.Verify(ch => ch.Close(), Times.Once);
        }
    }
}
