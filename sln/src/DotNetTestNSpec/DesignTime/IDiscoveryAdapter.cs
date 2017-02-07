using DotNetTestNSpec.Proxy;

namespace DotNetTestNSpec.DesignTime
{
    public interface IDiscoveryAdapter
    {
        void TestFound(DiscoveredExample example);
    }
}
