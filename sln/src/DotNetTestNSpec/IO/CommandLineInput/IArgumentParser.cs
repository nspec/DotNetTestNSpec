using DotNetTestNSpec.Domain;

namespace DotNetTestNSpec.IO.CommandLineInput
{
    public interface IArgumentParser
    {
        LaunchOptions Parse(string[] args);
    }
}
