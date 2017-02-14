using System;
using System.Collections.Generic;
using DotNetTestNSpec.Network;
using Microsoft.Extensions.Testing.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DotNetTestNSpec.DesignTime
{
    public class ExecutionAdapter : IExecutionAdapter
    {
        public ExecutionAdapter(INetworkChannel channel)
        {
            this.channel = channel;
        }

        public IEnumerable<string> Connect()
        {
            channel.Open();

            SendMessage(new Message()
            {
                MessageType = waitCommandMessageType,
            });

            string jsonReceived = channel.Receive();

            Message receivedMessage;

            try
            {
                receivedMessage = JsonConvert.DeserializeObject<Message>(jsonReceived);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Could not deserialize received message as a 'Message' instance. Received message:\n${jsonReceived}\nException:");
                Console.WriteLine(ex);

                receivedMessage = new Message();
            }

            IEnumerable<string> testNames;

            try
            {
                var runTestsMessage = receivedMessage.Payload.ToObject<RunTestsMessage>();

                testNames = runTestsMessage.Tests;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Could not deserialize received payload as a 'RunTestsMessage' instance. Received payload:\n${receivedMessage.Payload}\nException:");
                Console.WriteLine(ex);

                testNames = new string[0];
            }

            return testNames;
        }

        public void TestStarted(Test test)
        {
            SendMessage(new Message()
            {
                MessageType = testStarteMessageType,
                Payload = JToken.FromObject(test),
            });
        }

        public void TestFinished(TestResult testResult)
        {
            SendMessage(new Message()
            {
                MessageType = testResultMessageType,
                Payload = JToken.FromObject(testResult),
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

        const string waitCommandMessageType = "TestRunner.WaitCommand";
        const string testStarteMessageType = "TestExecution.TestStarted";
        const string testResultMessageType = "TestExecution.TestResult";
        const string testCompletedMessageType = "TestRunner.TestCompleted";
    }
}
