using DotNetTestNSpec.ConsoleTime;
using DotNetTestNSpec.DesignTime;
using DotNetTestNSpec.Dev.Network;
using DotNetTestNSpec.Network;
using DotNetTestNSpec.Parsing;
using DotNetTestNSpec.Proxy;
using System;

namespace DotNetTestNSpec
{
    public class RunnerFactory
    {
        public RunnerFactory(IArgumentParser argumentParser, IProxyFactory proxyFactory)
        {
            this.argumentParser = argumentParser;
            this.proxyFactory = proxyFactory;
        }

        public ITestRunner Create(string[] args)
        {
            var commandLineOptions = argumentParser.Parse(args);

            string testAssemblyPath = commandLineOptions.DotNet.Project;

            if (testAssemblyPath == null)
            {
                throw new DotNetTestNSpecException("Command line arguments must include path of test project assembly");
            }

            var controllerProxy = proxyFactory.Create(testAssemblyPath);

            if (!commandLineOptions.DotNet.DesignTime)
            {
                return new ConsoleRunner(testAssemblyPath, commandLineOptions.NSpec, controllerProxy);
            }

            if (!commandLineOptions.DotNet.Port.HasValue)
            {
                throw new DotNetTestNSpecException("Design-time command line arguments must include TCP port to connect to");
            }

            var channel = commandLineOptions.NSpec.DebugChannel
                ? new ConsoleDebugChannel() as INetworkChannel
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

        readonly IArgumentParser argumentParser;
        readonly IProxyFactory proxyFactory;
    }
}
