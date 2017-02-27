using DotNetTestNSpec.Shared;
using System.Collections.Generic;

namespace DotNetTestNSpec.Domain
{
    public class LaunchOptions
    {
        public LaunchOptions()
        { }

        public LaunchOptions(
            DotNetPart dotNetOptions,
            NSpecPart nspecOptions,
            IEnumerable<string> unknownArgs)
        {
            DotNet = new DotNetPart(dotNetOptions);
            NSpec = new NSpecPart(nspecOptions);
            UnknownArgs = unknownArgs;
        }

        public DotNetPart DotNet { get; set; }

        public NSpecPart NSpec { get; set; }

        public IEnumerable<string> UnknownArgs { get; set; }

        public override string ToString()
        {
            return EnumerableUtils.ToObjectString(new string[]
            {
                $"{nameof(DotNet)}: {DotNet}",
                $"{nameof(NSpec)}: {NSpec}",
                $"{nameof(UnknownArgs)}: {EnumerableUtils.ToArrayString(UnknownArgs)}",
            }, true);
        }

        public class DotNetPart
        {
            public DotNetPart()
            { }

            public DotNetPart(DotNetPart source)
            {
                Project = source.Project;
                DesignTime = source.DesignTime;
                List = source.List;
                WaitCommand = source.WaitCommand;
                ParentProcessId = source.ParentProcessId;
                Port = source.Port;
            }

            public string Project { get; set; }

            public bool DesignTime { get; set; }

            public bool List { get; set; }

            public bool WaitCommand { get; set; }

            public int? ParentProcessId { get; set; }

            public int? Port { get; set; }

            public override string ToString()
            {
                return EnumerableUtils.ToObjectString(new string[]
                {
                    $"{nameof(Project)}: {Project}",
                    $"{nameof(DesignTime)}: {DesignTime}",
                    $"{nameof(List)}: {List}",
                    $"{nameof(WaitCommand)}: {WaitCommand}",
                    $"{nameof(ParentProcessId)}: {ParentProcessId}",
                    $"{nameof(Port)}: {Port}",
                }, true);
            }
        }

        public class NSpecPart
        {
            public NSpecPart()
            { }

            public NSpecPart(NSpecPart source)
            {
                ClassName = source.ClassName;
                Tags = source.Tags;
                FailFast = source.FailFast;
                FormatterName = source.FormatterName;
                FormatterOptions = source.FormatterOptions;
                DebugChannel = source.DebugChannel;
                DebugTests = source.DebugTests;
            }

            public string ClassName { get; set; }

            public string Tags { get; set; }

            public bool FailFast { get; set; }

            public string FormatterName { get; set; }

            public Dictionary<string, string> FormatterOptions { get; set; }

            public bool DebugChannel { get; set; }

            public string[] DebugTests { get; set; }

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
                }, true);
            }
        }
    }
}
