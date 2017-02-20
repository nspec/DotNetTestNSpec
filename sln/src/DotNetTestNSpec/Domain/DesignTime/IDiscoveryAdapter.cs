using Microsoft.Extensions.Testing.Abstractions;

namespace DotNetTestNSpec.Domain.DesignTime
{
    public interface IDiscoveryAdapter
    {
        void Connect();

        void TestFound(Test test);

        void Disconnect();
    }
}
