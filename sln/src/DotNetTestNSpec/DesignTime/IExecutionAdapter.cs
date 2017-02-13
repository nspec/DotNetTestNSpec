using Microsoft.Extensions.Testing.Abstractions;
using System.Collections.Generic;

namespace DotNetTestNSpec.DesignTime
{
    public interface IExecutionAdapter
    {
        IEnumerable<string> Connect();

        void TestStarted(Test test);

        void TestFinished(TestResult testResult);

        void Disconnect();
    }
}
