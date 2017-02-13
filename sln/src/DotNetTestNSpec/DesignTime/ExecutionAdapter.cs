using System;
using System.Collections.Generic;
using DotNetTestNSpec.Network;
using Microsoft.Extensions.Testing.Abstractions;

namespace DotNetTestNSpec.DesignTime
{
    public class ExecutionAdapter : IExecutionAdapter
    {
        public ExecutionAdapter(INetworkChannel channel)
        {
            this.channel = channel;
        }

        public IEnumerable<string> Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void TestFinished(TestResult testResult)
        {
            throw new NotImplementedException();
        }

        public void TestStarted(Test test)
        {
            throw new NotImplementedException();
        }

        readonly INetworkChannel channel;
    }
}
