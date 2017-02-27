using DotNetTestNSpec.Shared;
using System;

namespace DotNetTestNSpec.Domain.Library
{
    public class DiscoveredExample
    {
        public string FullName { get; set; }

        public string SourceFilePath { get; set; }

        public int SourceLineNumber { get; set; }

        public string SourceAssembly { get; set; }

        public string[] Tags { get; set; }

        public override string ToString()
        {
            string tagsText = String.Join(", ", Tags);

            return EnumerableUtils.ToObjectString(new string[]
            {
                $"{nameof(FullName)}: {FullName}",
                $"{nameof(SourceFilePath)}: {SourceFilePath}",
                $"{nameof(SourceLineNumber)}: {SourceLineNumber}",
                $"{nameof(SourceAssembly)}: {SourceAssembly}",
                $"{nameof(Tags)}: [{tagsText}]",

            }, true);
        }
    }
}
