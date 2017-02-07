using DotNetTestNSpec.Proxy;

namespace DotNetTestNSpec.DesignTime
{
    public class DiscoveryRunner : ITestRunner
    {
        public DiscoveryRunner(CommandLineOptions commandLineOptions, IControllerProxy controllerProxy,
            IDiscoveryAdapter adapter)
        {
            this.commandLineOptions = commandLineOptions;
            this.controllerProxy = controllerProxy;
            this.adapter = adapter;
        }

        public int Start()
        {
            adapter.Connect();

            var discoveredExamples = controllerProxy.List(commandLineOptions.DotNet.Project);

            foreach (var example in discoveredExamples)
            {
                adapter.TestFound(example);
            }

            adapter.Disconnect();

            return dontCare;
        }

        readonly CommandLineOptions commandLineOptions;
        readonly IControllerProxy controllerProxy;
        readonly IDiscoveryAdapter adapter;

        const int dontCare = -1;
    }
}
