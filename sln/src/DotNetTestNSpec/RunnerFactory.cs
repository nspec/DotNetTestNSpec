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

            string testAssemblyPath = commandLineOptions.DotNet.Project;

            if (testAssemblyPath == null)
            {
                throw new DotNetTestNSpecException("Command line arguments must include path of test project assembly");
            }

            var controllerProxy = proxyFactory.Create(testAssemblyPath);

            ITestRunner runner;

            if (!commandLineOptions.DotNet.DesignTime)
            {
                runner = new ConsoleRunner(testAssemblyPath, commandLineOptions.NSpec, controllerProxy);
            }
            else if (commandLineOptions.DotNet.List)
            {
                runner = new DiscoveryRunner(testAssemblyPath, controllerProxy, new DiscoveryAdapter());
            }
            else if (commandLineOptions.DotNet.WaitCommand)
            {
                runner = new ExecutionRunner(testAssemblyPath, controllerProxy);
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
