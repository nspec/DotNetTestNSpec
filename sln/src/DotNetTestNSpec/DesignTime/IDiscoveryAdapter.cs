using Microsoft.Extensions.Testing.Abstractions;

namespace DotNetTestNSpec.DesignTime
{
    public interface IDiscoveryAdapter
    {
        void Connect();

        void TestFound(Test test);

        void Disconnect();
    }
}
