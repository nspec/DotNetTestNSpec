using DotNetTestNSpec.ConsoleTime;
using DotNetTestNSpec.DesignTime;
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

            if (commandLineOptions.DotNet.Project == null)
            {
                throw new DotNetTestNSpecException("Command line arguments must include path of test project assembly");
            }

            var controllerProxy = proxyFactory.Create(commandLineOptions.DotNet.Project);

            ITestRunner runner;

            if (!commandLineOptions.DotNet.DesignTime)
            {
                runner = new ConsoleRunner(commandLineOptions, controllerProxy);
            }
            else if (commandLineOptions.DotNet.List)
            {
                runner = new DiscoveryRunner(commandLineOptions, controllerProxy);
            }
            else if (commandLineOptions.DotNet.WaitCommand)
            {
                runner = new ExecutionRunner(commandLineOptions, controllerProxy);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(args), args,
                    $"Unknown command line argument combination: cannot figure out which test runner should run");
            }

            return runner;
        }

        readonly IArgumentParser argumentParser;
        readonly IProxyFactory proxyFactory;
    }
}
