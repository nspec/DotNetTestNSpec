using DotNetTestNSpec.Domain.Library;
using Microsoft.Extensions.Testing.Abstractions;
using System.Collections.Generic;

namespace DotNetTestNSpec.Domain.DesignTime
{
    public class ExecutionRunner : ITestRunner
    {
        public ExecutionRunner(string testAssemblyPath, IControllerProxy controllerProxy,
            IExecutionAdapter adapter)
        {
            this.testAssemblyPath = testAssemblyPath;
            this.controllerProxy = controllerProxy;
            this.adapter = adapter;
        }

        public int Start()
        {
            using (var connection = adapter.Connect())
            {
                var requestedTestFullNames = connection.GetTests();

                var sink = new Sink(connection);

                controllerProxy.RunInteractive(testAssemblyPath, requestedTestFullNames, sink);
            }

            return dontCare;
        }

        readonly string testAssemblyPath;
        readonly IControllerProxy controllerProxy;
        readonly IExecutionAdapter adapter;

        const int dontCare = -1;

        public class Sink : IExecutionSink
        {
            public Sink(IExecutionConnection connection)
            {
                this.connection = connection;

                exampleMapper = new ExampleMapper();

                startedTestMap = new Dictionary<string, Test>();
            }

            public void ExampleStarted(DiscoveredExample example)
            {
                var test = exampleMapper.MapToTest(example);

                startedTestMap[example.FullName] = test;

                connection.TestStarted(test);
            }

            public void ExampleCompleted(ExecutedExample example)
            {
                var test = startedTestMap[example.FullName];

                var testResult = exampleMapper.MapToTestResult(example, test);

                connection.TestFinished(testResult);
            }

            readonly IExecutionConnection connection;
            readonly ExampleMapper exampleMapper;
            readonly IDictionary<string, Test> startedTestMap;
        }
    }
}
