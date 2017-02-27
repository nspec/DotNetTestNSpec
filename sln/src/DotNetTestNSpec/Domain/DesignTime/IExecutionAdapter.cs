using Microsoft.Extensions.Testing.Abstractions;
using System;
using System.Collections.Generic;

namespace DotNetTestNSpec.Domain.DesignTime
{
    public interface IExecutionAdapter
    {
        IExecutionConnection Connect();
    }

    public interface IExecutionConnection : IDisposable
    {
        IEnumerable<string> GetTests();

        void TestStarted(Test test);

        void TestFinished(TestResult testResult);
    }
}
