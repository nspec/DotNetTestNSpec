using System.Linq;

namespace DotNetTestNSpec.Parsing
{
    public class ArgumentParser : IArgumentParser
    {
        public CommandLineOptions Parse(string[] args)
        {
            var dotNetArgumentParser = new DotNetArgumentParser();

            DotNetCommandLineOptions dotNetOptions = dotNetArgumentParser.Parse(args);

            var nspecArgumentParser = new NSpecArgumentParser();

            NSpecCommandLineOptions nspecOptions = nspecArgumentParser.Parse(dotNetOptions.NSpecArgs);

            var allUnknownArgs = dotNetOptions.UnknownArgs
                .Concat(nspecOptions.UnknownArgs);

            var options = new CommandLineOptions(dotNetOptions, nspecOptions, allUnknownArgs);

            return options;
        }
    }
}
