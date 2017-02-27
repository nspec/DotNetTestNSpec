using Microsoft.Extensions.Testing.Abstractions;
using System;

namespace DotNetTestNSpec.Domain.DesignTime
{
    public interface IDiscoveryAdapter
    {
        IDiscoveryConnection Connect();
    }

    public interface IDiscoveryConnection : IDisposable
    {
        void TestFound(Test test);
    }
}
