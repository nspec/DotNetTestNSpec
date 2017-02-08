namespace DotNetTestNSpec.Network
{
    public interface INetworkChannel
    {
        void Open();

        void Send(string message);

        void Close();
    }
}
