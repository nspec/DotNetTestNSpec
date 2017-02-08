using System;
using Microsoft.Extensions.Testing.Abstractions;

namespace DotNetTestNSpec.DesignTime
{
    public class DiscoveryAdapter : IDiscoveryAdapter
    {
        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void TestFound(Test test)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
    }
}
