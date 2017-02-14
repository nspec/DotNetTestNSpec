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

            binaryWriter = new BinaryWriter(networkStream);
            binaryReader = new BinaryReader(networkStream);
        }

        public void Send(string message)
        {
            binaryWriter.Write(message);
        }

        public string Receive()
        {
            return binaryReader.ReadString();
        }

        public void Close()
        {
            DisposeReader();
            DisposeWriter();
            DisposeNetwork();
            DisposeSocket();
        }

        void DisposeReader()
        {
#if NET451
            binaryReader.Close();
#endif
            binaryReader.Dispose();
        }

        void DisposeWriter()
        {
            binaryWriter.Flush();
#if NET451
            binaryWriter.Close();
#endif
            binaryWriter.Dispose();
        }

        private void DisposeNetwork()
        {
            networkStream.Flush();
#if NET451
            networkStream.Close();
#endif
            networkStream.Dispose();
        }

        private void DisposeSocket()
        {
            socket.Shutdown(SocketShutdown.Both);
#if NET451
            socket.Close();
#endif
            socket.Dispose();
        }

        Socket socket;
        NetworkStream networkStream;
        BinaryWriter binaryWriter;
        BinaryReader binaryReader;

        readonly int port;
    }
}
