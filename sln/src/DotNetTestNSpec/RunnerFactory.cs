using DotNetTestNSpec.ConsoleTime;
using DotNetTestNSpec.DesignTime;
using DotNetTestNSpec.Dev.Network;
using DotNetTestNSpec.Network;
using DotNetTestNSpec.Proxy;

namespace DotNetTestNSpec
{
    public class RunnerFactory
    {
        public RunnerFactory(IProxyFactory proxyFactory)
        {
            this.proxyFactory = proxyFactory;
        }

        public ITestRunner Create(CommandLineOptions commandLineOptions)
        {
            string testAssemblyPath = commandLineOptions.DotNet.Project;

            if (testAssemblyPath == null)
            {
                throw new DotNetTestNSpecException("Command line arguments must include path of test project assembly");
            }

            var controllerProxy = proxyFactory.Create(testAssemblyPath);

            if (!commandLineOptions.DotNet.Port.HasValue)
            {
                return new ConsoleRunner(testAssemblyPath, commandLineOptions.NSpec, controllerProxy);
            }

            var channel = commandLineOptions.NSpec.DebugChannel
                ? new ConsoleDebugChannel(commandLineOptions.NSpec.DebugTests) as INetworkChannel
                : new NetworkChannel(commandLineOptions.DotNet.Port.Value);

            ITestRunner runner;

            if (commandLineOptions.DotNet.List)
            {
                var adapter = new DiscoveryAdapter(channel);

                runner = new DiscoveryRunner(testAssemblyPath, adapter, controllerProxy);
            }
            else if (commandLineOptions.DotNet.WaitCommand)
            {
                var adapter = new ExecutionAdapter(channel);

                runner = new ExecutionRunner(testAssemblyPath, adapter, controllerProxy);
            }
            else
            {
                throw new DotNetTestNSpecException("Design-time command line argument must include either list or wait command options");
            }

            return runner;
        }

        readonly IProxyFactory proxyFactory;
    }
}
