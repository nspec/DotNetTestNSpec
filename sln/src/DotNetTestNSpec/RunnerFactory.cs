using DotNetTestNSpec.ConsoleTime;
using DotNetTestNSpec.DesignTime;
using DotNetTestNSpec.Parsing;

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

            var runner = commandLineOptions.DotNet.DesignTime
                ? new DesignTimeRunner(commandLineOptions) as ITestRunner
                : new ConsoleRunner(commandLineOptions) as ITestRunner;

            return runner;
        }

        readonly IArgumentParser argumentParser;
    }
}
