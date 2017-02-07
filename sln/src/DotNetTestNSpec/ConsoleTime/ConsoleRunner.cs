using DotNetTestNSpec.Proxy;

namespace DotNetTestNSpec.ConsoleTime
{
    public class ConsoleRunner : ITestRunner
    {
        public ConsoleRunner(CommandLineOptions commandLineOptions, IControllerProxy controllerProxy)
        {
            this.commandLineOptions = commandLineOptions;
            this.controllerProxy = controllerProxy;
        }

        public int Start()
        {
            int nrOfFailures = controllerProxy.Run(
                testAssemblyPath: commandLineOptions.DotNet.Project,
                tags: commandLineOptions.NSpec.Tags,
                formatterClassName: commandLineOptions.NSpec.FormatterName,
                formatterOptions: commandLineOptions.NSpec.FormatterOptions,
                failFast: commandLineOptions.NSpec.FailFast);

            return nrOfFailures;
        }

        readonly CommandLineOptions commandLineOptions;
        readonly IControllerProxy controllerProxy;
    }
}
