using DotNetTestNSpec.DesignTime;
using DotNetTestNSpec.Network;
using FluentAssertions;
using Microsoft.Extensions.Testing.Abstractions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DotNetTestNSpec.Tests.DesignTime.Execution
{
    [TestFixture]
    [Category("ExecutionAdapter")]
    public abstract class describe_ExecutionAdapter
    {
        protected ExecutionAdapter adapter;

        protected Mock<INetworkChannel> channel;
        protected List<string> sentMessages;

        protected readonly Test someTest = new Test()
        {
            FullyQualifiedName = "nspec. A class. A context. Some Example",
            DisplayName = "A class › A context › Some Example",
            CodeFilePath = @"some\path\to\code",
            LineNumber = 10,
        };

        [SetUp]
        public virtual void setup()
        {
            channel = new Mock<INetworkChannel>();

            channel.Setup(ch => ch.Send(It.IsAny<string>())).Callback((string message) =>
            {
                sentMessages.Add(message);
            });

            sentMessages = new List<string>();

            adapter = new ExecutionAdapter(channel.Object);
        }
    }

    public class when_connected : describe_ExecutionAdapter
    {
        IEnumerable<string> actuals;

        readonly string[] requestedTestNames =
        {
            "Some test name 1",
            "Some test name 2",
            "Some test name 3",
        };

        readonly string receivedMessage;

        public when_connected()
        {
            var runTestsMessage = new RunTestsMessage()
            {
                Tests = new List<string>(requestedTestNames),
            };

            var message = new Message()
            {
                MessageType = "TestRunner.Execute",
                Payload = JToken.FromObject(runTestsMessage),
            };

            receivedMessage = JsonConvert.SerializeObject(message);
        }

        public override void setup()
        {
            base.setup();

            channel.Setup(ch => ch.Receive()).Returns(receivedMessage);

            actuals = adapter.Connect();
        }

        [TestCase]
        public void it_should_open_channel()
        {
            channel.Verify(ch => ch.Open(), Times.Once);
        }

        [TestCase]
        public void it_should_send_wait_command_message()
        {
            string expected = JsonConvert.SerializeObject(new Message()
            {
                MessageType = "TestRunner.WaitCommand",
                Payload = null,
            });

            sentMessages.ShouldBeEquivalentTo(new[] { expected });
        }

        [TestCase]
        public void it_should_return_test_names()
        {
            actuals.ShouldBeEquivalentTo(requestedTestNames);
        }
    }

    public class when_test_started : describe_ExecutionAdapter
    {
        public override void setup()
        {
            base.setup();

            adapter.TestStarted(someTest);
        }

        [TestCase]
        public void it_should_send_test_started_message()
        {
            string expected = JsonConvert.SerializeObject(new Message()
            {
                MessageType = "TestExecution.TestStarted",
                Payload = JToken.FromObject(someTest),
            });

            sentMessages.ShouldBeEquivalentTo(new[] { expected });
        }
    }

    public class when_test_finished : describe_ExecutionAdapter
    {
        readonly TestResult someTestResult;

        public when_test_finished()
        {
            someTestResult = new TestResult(someTest)
            {
                DisplayName = someTest.DisplayName,
                ComputerName = "Some computer name",
                ErrorMessage = "Some error message",
                ErrorStackTrace = "Some error stacktrace",
                Outcome = TestOutcome.Failed,
                StartTime = new DateTimeOffset(new DateTime(2017, 6, 20, 10, 30, 15)),
                EndTime = new DateTimeOffset(new DateTime(2017, 6, 20, 10, 31, 15)),
            };
        }

        public override void setup()
        {
            base.setup();

            adapter.TestFinished(someTestResult);
        }

        [TestCase]
        public void it_should_send_test_result_message()
        {
            string expected = JsonConvert.SerializeObject(new Message()
            {
                MessageType = "TestExecution.TestResult",
                Payload = JToken.FromObject(someTestResult),
            });

            sentMessages.ShouldBeEquivalentTo(new[] { expected });
        }
    }

    public class when_disconnected : describe_ExecutionAdapter
    {
        public override void setup()
        {
            base.setup();

            adapter.Disconnect();
        }

        [TestCase]
        public void it_should_send_test_completed_message()
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
