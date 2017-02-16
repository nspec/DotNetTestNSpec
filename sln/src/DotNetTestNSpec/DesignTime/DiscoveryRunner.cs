using DotNetTestNSpec.Proxy;

namespace DotNetTestNSpec.DesignTime
{
    public class DiscoveryRunner : ITestRunner
    {
        public DiscoveryRunner(string testAssemblyPath, IControllerProxy controllerProxy,
            IDiscoveryAdapter adapter)
        {
            this.testAssemblyPath = testAssemblyPath;
            this.controllerProxy = controllerProxy;
            this.adapter = adapter;
        }

        public int Start()
        {
            adapter.Connect();

            var discoveredExamples = controllerProxy.List(testAssemblyPath);

            foreach (var example in discoveredExamples)
            {
                var test = MappingUtils.MapToTest(example);

                adapter.TestFound(test);
            }

            adapter.Disconnect();

            return dontCare;
        }

        readonly string testAssemblyPath;
        readonly IControllerProxy controllerProxy;
        readonly IDiscoveryAdapter adapter;

        const int dontCare = -1;
    }
}
