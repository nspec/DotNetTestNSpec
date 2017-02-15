using DotNetTestNSpec.Network;
using Microsoft.Extensions.Testing.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace DotNetTestNSpec.Dev.Network
{
    public class ConsoleDebugChannel : INetworkChannel
    {
        public ConsoleDebugChannel(IEnumerable<string> debugTests)
        {
            this.debugTests = debugTests;
        }

        public void Open()
        {
            Console.WriteLine(nameof(Open));
        }

        public void Send(string message)
        {
            string loggedMessage;

            try
            {
                var originalMessage = JsonConvert.DeserializeObject<Message>(message);

                loggedMessage = JsonConvert.SerializeObject(originalMessage, jsonSettings);
            }
            catch (Exception)
            {
                loggedMessage = message;
            }

            Console.WriteLine($"{nameof(Send)}");
            Console.WriteLine("-- BEGIN --");
            Console.WriteLine(loggedMessage);
            Console.WriteLine("--- END ---");
        }

        public string Receive()
        {
            var runTestsMessage = new RunTestsMessage()
            {
                Tests = new List<string>(debugTests),
            };

            var message = new Message()
            {
                MessageType = "TestRunner.Execute",
                Payload = JToken.FromObject(runTestsMessage),
            };

            string receivedMessage = JsonConvert.SerializeObject(message);

            string loggedMessage = JsonConvert.SerializeObject(message, jsonSettings);

            Console.WriteLine($"{nameof(Receive)}");
            Console.WriteLine("-- BEGIN --");
            Console.WriteLine(loggedMessage);
            Console.WriteLine("--- END ---");

            return receivedMessage;
        }

        public void Close()
        {
            Console.WriteLine(nameof(Close));
        }

        readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            Converters = new[] { new StringEnumConverter() },
        };

        readonly IEnumerable<string> debugTests;
    }
}
