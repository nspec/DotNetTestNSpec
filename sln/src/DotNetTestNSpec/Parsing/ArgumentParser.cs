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

            var options = new CommandLineOptions(dotNetOptions, nspecOptions, nspecOptions.UnknownArgs);

            return options;
        }
    }
}
