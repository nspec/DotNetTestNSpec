namespace DotNetTestNSpec
{
    public class NSpecCommandLineOptions : CommandLineOptions.NSpecPart
    {
        public NSpecCommandLineOptions()
        { }

        public NSpecCommandLineOptions(NSpecCommandLineOptions source)
            : base(source)
        {
            UnknownArgs = source.UnknownArgs;
        }

        public string[] UnknownArgs { get; set; }

        public override string ToString()
        {
            return EnumerableUtils.ToObjectString(new string[]
            {
                $"{nameof(ClassName)}: {ClassName}",
                $"{nameof(Tags)}: {Tags}",
                $"{nameof(FailFast)}: {FailFast}",
                $"{nameof(FormatterName)}: {FormatterName}",
                $"{nameof(FormatterOptions)}: {DictionaryUtils.ToArrayString(FormatterOptions)}",
                $"{nameof(DebugChannel)}: {DebugChannel}",
                $"{nameof(DebugTests)}: {EnumerableUtils.ToArrayString(DebugTests)}",
                $"{nameof(UnknownArgs)}: {EnumerableUtils.ToArrayString(UnknownArgs)}",

            }, true);
        }
    }
}
