using System;
using DotNetTestNSpec.Proxy;

namespace DotNetTestNSpec.DesignTime
{
    public class DiscoveryAdapter : IDiscoveryAdapter
    {
        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void TestFound(DiscoveredExample example)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }
    }
}
