namespace DotNetTestNSpec
{
    public class DotNetCommandLineOptions : CommandLineOptions.DotNetPart
    {
        public string[] NSpecArgs { get; set; }

        public string[] UnknownArgs { get; set; }

        public override string ToString()
        {
            return EnumerableUtils.ToObjectString(new string[]
            {
                $"{nameof(Project)}: {Project}",
                $"{nameof(ParentProcessId)}: {ParentProcessId}",
                $"{nameof(Port)}: {Port}",
                $"{nameof(NSpecArgs)}: {EnumerableUtils.ToArrayString(NSpecArgs)}",
                $"{nameof(UnknownArgs)}: {EnumerableUtils.ToArrayString(UnknownArgs)}",
            }, true);
        }
    }
}
