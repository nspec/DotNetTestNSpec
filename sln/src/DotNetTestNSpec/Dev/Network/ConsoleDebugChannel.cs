using DotNetTestNSpec.Network;
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
            Console.WriteLine($"{nameof(Send)}");
            Console.WriteLine("-- BEGIN --");
            Console.WriteLine(message);
            Console.WriteLine("--- END ---");
        }

        public void Close()
        {
            Console.WriteLine(nameof(Close));
        }
    }
}
