using DotNetTestNSpec.Domain.Library;

namespace DotNetTestNSpec.Domain.DesignTime
{
    public class DiscoveryRunner : ITestRunner
    {
        public DiscoveryRunner(string testAssemblyPath, IControllerProxy controllerProxy,
            IDiscoveryAdapter adapter)
        {
            this.testAssemblyPath = testAssemblyPath;
            this.controllerProxy = controllerProxy;
            this.adapter = adapter;

            exampleMapper = new ExampleMapper();
        }

        public int Start()
        {
            using (var connection = adapter.Connect())
            {
                var discoveredExamples = controllerProxy.List(testAssemblyPath);

                foreach (var example in discoveredExamples)
                {
                    var test = exampleMapper.MapToTest(example);

                    connection.TestFound(test);
                }
            }

            return dontCare;
        }

        readonly string testAssemblyPath;
        readonly IControllerProxy controllerProxy;
        readonly IDiscoveryAdapter adapter;
        readonly ExampleMapper exampleMapper;

        const int dontCare = -1;
    }
}
