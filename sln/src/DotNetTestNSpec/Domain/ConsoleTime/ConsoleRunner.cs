using DotNetTestNSpec.Domain.Library;

namespace DotNetTestNSpec.Domain.ConsoleTime
{
    public class ConsoleRunner : ITestRunner
    {
        public ConsoleRunner(string testAssemblyPath, IControllerProxy controllerProxy,
            LaunchOptions.NSpecPart nspecOptions)
        {
            this.testAssemblyPath = testAssemblyPath;
            this.controllerProxy = controllerProxy;
            this.nspecOptions = nspecOptions;
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
        readonly IControllerProxy controllerProxy;
        readonly LaunchOptions.NSpecPart nspecOptions;
    }
}
