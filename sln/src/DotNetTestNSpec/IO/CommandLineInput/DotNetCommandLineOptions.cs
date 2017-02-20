using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Shared;

namespace DotNetTestNSpec.IO.CommandLineInput
{
    public class DotNetCommandLineOptions : LaunchOptions.DotNetPart
    {
        public DotNetCommandLineOptions()
        { }

        public DotNetCommandLineOptions(DotNetCommandLineOptions source)
            : base(source)
        {
            NSpecArgs = source.NSpecArgs;
            UnknownArgs = source.UnknownArgs;
        }

        public string[] NSpecArgs { get; set; }

        public string[] UnknownArgs { get; set; }

        public override string ToString()
        {
            return EnumerableUtils.ToObjectString(new string[]
            {
                $"{nameof(Project)}: {Project}",
                $"{nameof(DesignTime)}: {DesignTime}",
                $"{nameof(List)}: {List}",
                $"{nameof(ParentProcessId)}: {ParentProcessId}",
                $"{nameof(Port)}: {Port}",
                $"{nameof(NSpecArgs)}: {EnumerableUtils.ToArrayString(NSpecArgs)}",
                $"{nameof(UnknownArgs)}: {EnumerableUtils.ToArrayString(UnknownArgs)}",
            }, true);
        }
    }
}
