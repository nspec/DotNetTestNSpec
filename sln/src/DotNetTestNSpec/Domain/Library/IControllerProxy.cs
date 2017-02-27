using System.Collections.Generic;

namespace DotNetTestNSpec.Domain.Library
{
    public interface IControllerProxy
    {
        int RunBatch(
            string testAssemblyPath,
            string tags,
            string formatterClassName,
            IDictionary<string, string> formatterOptions,
            bool failFast);

        IEnumerable<DiscoveredExample> List(string testAssemblyPath);

        void RunInteractive(
            string testAssemblyPath,
            IEnumerable<string> exampleFullNames,
            IExecutionSink sink);
    }
}
