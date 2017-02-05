namespace DotNetTestNSpec.Parsing
{
    public interface IArgumentParser
    {
        CommandLineOptions Parse(string[] args);
    }
}
