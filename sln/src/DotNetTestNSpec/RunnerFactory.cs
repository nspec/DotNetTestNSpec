using DotNetTestNSpec.ConsoleTime;
using DotNetTestNSpec.DesignTime;
using DotNetTestNSpec.Parsing;
using System;

namespace DotNetTestNSpec
{
    public class RunnerFactory
    {
        public RunnerFactory(IArgumentParser argumentParser)
        {
            this.argumentParser = argumentParser;
        }

        public ITestRunner Create(string[] args)
        {
            var commandLineOptions = argumentParser.Parse(args);

            ITestRunner runner;

            if (!commandLineOptions.DotNet.DesignTime)
            {
                runner = new ConsoleRunner(commandLineOptions);
            }
            else if (commandLineOptions.DotNet.List)
            {
                runner = new DiscoveryRunner(commandLineOptions);
            }
            else if (commandLineOptions.DotNet.WaitCommand)
            {
                runner = new ExecutionRunner(commandLineOptions);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(args), args,
                    $"Unknown command line argument combination: cannot figure out which test runner should run");
            }

            return runner;
        }

        readonly IArgumentParser argumentParser;
    }
}
