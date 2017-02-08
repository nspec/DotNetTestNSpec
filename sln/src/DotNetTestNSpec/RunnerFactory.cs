using DotNetTestNSpec.ConsoleTime;
using DotNetTestNSpec.DesignTime;
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

            ITestRunner runner;

            if (commandLineOptions.DotNet.List)
            {
                var channel = new NetworkChannel(commandLineOptions.DotNet.Port.Value);
                var adapter = new DiscoveryAdapter(channel);

                runner = new DiscoveryRunner(testAssemblyPath, adapter, controllerProxy);
            }
            else if (commandLineOptions.DotNet.WaitCommand)
            {
                runner = new ExecutionRunner(testAssemblyPath, controllerProxy);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(args), args,
                    "Unknown command line argument combination: cannot figure out which test runner should run");
            }

            return runner;
        }

        readonly IArgumentParser argumentParser;
        readonly IProxyFactory proxyFactory;
    }
}
