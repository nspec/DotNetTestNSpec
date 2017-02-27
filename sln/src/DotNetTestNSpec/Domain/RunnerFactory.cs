using DotNetTestNSpec.Domain.ConsoleTime;
using DotNetTestNSpec.Domain.DesignTime;
using DotNetTestNSpec.Domain.Library;
using DotNetTestNSpec.Domain.VisualStudio;

namespace DotNetTestNSpec.Domain
{
    public class RunnerFactory
    {
        public RunnerFactory(IProxyFactory proxyFactory, IChannelFactory channelFactory)
        {
            this.proxyFactory = proxyFactory;
            this.channelFactory = channelFactory;
        }

        public ITestRunner Create(LaunchOptions options)
        {
            string testAssemblyPath = options.DotNet.Project;

            if (testAssemblyPath == null)
            {
                throw new DotNetTestNSpecException("Command line arguments must include path of test project assembly");
            }

            var controllerProxy = proxyFactory.Create(testAssemblyPath);

            if (!options.DotNet.Port.HasValue)
            {
                return new ConsoleRunner(testAssemblyPath, controllerProxy, options.NSpec);
            }

            ITestRunner runner;

            if (options.DotNet.List)
            {
                var adapter = new DiscoveryAdapter(channelFactory);

                runner = new DiscoveryRunner(testAssemblyPath, controllerProxy, adapter);
            }
            else if (options.DotNet.WaitCommand)
            {
                var adapter = new ExecutionAdapter(channelFactory);

                runner = new ExecutionRunner(testAssemblyPath, controllerProxy, adapter);
            }
            else
            {
                throw new DotNetTestNSpecException("Design-time command line argument must include either list or wait command options");
            }

            return runner;
        }

        readonly IProxyFactory proxyFactory;
        readonly IChannelFactory channelFactory;
    }
}
