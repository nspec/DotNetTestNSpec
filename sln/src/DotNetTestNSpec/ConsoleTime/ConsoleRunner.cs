using DotNetTestNSpec.Proxy;

namespace DotNetTestNSpec.ConsoleTime
{
    public class ConsoleRunner : ITestRunner
    {
        public ConsoleRunner(CommandLineOptions commandLineOptions, ProxyFactory proxyFactory)
        {
            this.commandLineOptions = commandLineOptions;
            this.proxyFactory = proxyFactory;
        }

        public int Start()
        {
            EnsureOptionsValid();

            var controllerProxy = proxyFactory.Create(commandLineOptions.DotNet.Project);

            int nrOfFailures = controllerProxy.Run(
                testAssemblyPath: commandLineOptions.DotNet.Project,
                tags: commandLineOptions.NSpec.Tags,
                formatterClassName: commandLineOptions.NSpec.FormatterName,
                formatterOptions: commandLineOptions.NSpec.FormatterOptions,
                failFast: commandLineOptions.NSpec.FailFast);

            return nrOfFailures;
        }

        void EnsureOptionsValid()
        {
            if (commandLineOptions.DotNet.Project == null)
            {
                throw new DotNetTestNSpecException("Command line arguments must include path of test project assembly");
            }
        }

        readonly CommandLineOptions commandLineOptions;
        readonly ProxyFactory proxyFactory;
    }
}
