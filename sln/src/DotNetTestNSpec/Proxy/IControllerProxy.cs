using System.Collections.Generic;

namespace DotNetTestNSpec.Proxy
{
    public interface IControllerProxy
    {
        int Run(
            string testAssemblyPath,
            string tags,
            string formatterClassName,
            IDictionary<string, string> formatterOptions,
            bool failFast);

        IEnumerable<DiscoveredExample> List(string testAssemblyPath);

        void Run(
            string testAssemblyPath,
            IEnumerable<string> exampleFullNames,
            IRunSink sink);
    }
}
