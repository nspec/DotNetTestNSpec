using System.IO;
using System.Net;
using System.Net.Sockets;

namespace DotNetTestNSpec.Network
{
    public class NetworkChannel : INetworkChannel
    {
        public NetworkChannel(int port)
        {
            this.port = port;
        }

        public void Open()
        {
            var endPoint = new IPEndPoint(IPAddress.Loopback, port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);

            networkStream = new NetworkStream(socket);

            binaryStream = new BinaryWriter(networkStream);
        }

        public void Send(string message)
        {
            binaryStream.Write(message);
        }

        public string Receive()
        {
            // TODO
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            binaryStream.Flush();
#if NET451
            binaryStream.Close();
#endif
            binaryStream.Dispose();

            networkStream.Flush();
#if NET451
            networkStream.Close();
#endif
            networkStream.Dispose();

            socket.Shutdown(SocketShutdown.Both);
#if NET451
            socket.Close();
#endif
            socket.Dispose();
        }

        Socket socket;
        NetworkStream networkStream;
        BinaryWriter binaryStream;

        readonly int port;
    }
}
