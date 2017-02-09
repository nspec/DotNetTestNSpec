using DotNetTestNSpec.Network;
using Microsoft.Extensions.Testing.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace DotNetTestNSpec.Dev.Network
{
    public class ConsoleDebugChannel : INetworkChannel
    {
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

        public void Close()
        {
            Console.WriteLine(nameof(Close));
        }

        readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            Converters = new[] { new StringEnumConverter() },
        };
    }
}
