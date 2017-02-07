using DotNetTestNSpec.Proxy;

namespace DotNetTestNSpec.ConsoleTime
{
    public class ConsoleRunner : ITestRunner
    {
        public ConsoleRunner(string testAssemblyPath, CommandLineOptions.NSpecPart nspecOptions,
            IControllerProxy controllerProxy)
        {
            this.testAssemblyPath = testAssemblyPath;
            this.nspecOptions = nspecOptions;
            this.controllerProxy = controllerProxy;
        }

        public int Start()
        {
            int nrOfFailures = controllerProxy.Run(
                testAssemblyPath: testAssemblyPath,
                tags: nspecOptions.Tags,
                formatterClassName: nspecOptions.FormatterName,
                formatterOptions: nspecOptions.FormatterOptions,
                failFast: nspecOptions.FailFast);

            return nrOfFailures;
        }

        readonly string testAssemblyPath;
        readonly CommandLineOptions.NSpecPart nspecOptions;
        readonly IControllerProxy controllerProxy;
    }
}
