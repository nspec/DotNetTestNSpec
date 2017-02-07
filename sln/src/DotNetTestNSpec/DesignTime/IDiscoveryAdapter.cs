using DotNetTestNSpec.Proxy;

namespace DotNetTestNSpec.DesignTime
{
    public interface IDiscoveryAdapter
    {
        void Connect();

        void TestFound(DiscoveredExample example);

        void Disconnect();
    }
}
