using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Domain.Library;
using DotNetTestNSpec.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetTestNSpec.IO.Library
{
    public class ControllerProxy : IControllerProxy
    {
        public ControllerProxy(Assembly nspecLibraryAssembly)
        {
            controller = CreateController(nspecLibraryAssembly);
        }

        public int RunBatch(
            string testAssemblyPath,
            string tags,
            string formatterClassName,
            IDictionary<string, string> formatterOptions,
            bool failFast)
        {
            object methodResult = InvokeMethod(controller, runBatchMethodName,
                testAssemblyPath, tags, formatterClassName, formatterOptions, failFast);

            int nrOfFailures = (int)methodResult;

            return nrOfFailures;
        }

        public IEnumerable<DiscoveredExample> List(string testAssemblyPath)
        {
            object methodResult = InvokeMethod(controller, listMethodName,
                testAssemblyPath);

            string jsonResult = (string)methodResult;

            DiscoveredExample[] examples;

            try
            {
                examples = JsonConvert.DeserializeObject<DiscoveredExample[]>(jsonResult);
            }
            catch (Exception ex)
            {
                throw new DotNetTestNSpecException(unknownResultErrorMessage.With(listMethodName, jsonResult), ex);
            }

            return examples;
        }

        public void RunInteractive(
            string testAssemblyPath,
            IEnumerable<string> exampleFullNames,
            IExecutionSink sink)
        {
            Action<string> onExampleStarted = jsonArg => OnExampleStarted(sink, jsonArg);
            Action<string> onExampleCompleted = jsonArg => OnExampleCompleted(sink, jsonArg);

            InvokeMethod(controller, runInteractiveMethodName,
                testAssemblyPath, exampleFullNames, onExampleStarted, onExampleCompleted);
        }

        static void OnExampleStarted(IExecutionSink sink, string jsonArg)
        {
            DiscoveredExample example;

            try
            {
                example = JsonConvert.DeserializeObject<DiscoveredExample>(jsonArg);
            }
            catch (Exception ex)
            {
                throw new DotNetTestNSpecException(unknownArgumentErrorMessage
                    .With(runInteractiveMethodName + ": " + nameof(OnExampleStarted), jsonArg), ex);
            }

            sink.ExampleStarted(example);
        }

        static void OnExampleCompleted(IExecutionSink sink, string jsonArg)
        {
            ExecutedExample example;

            try
            {
                example = JsonConvert.DeserializeObject<ExecutedExample>(jsonArg);
            }
            catch (Exception ex)
            {
                throw new DotNetTestNSpecException(unknownArgumentErrorMessage
                    .With(runInteractiveMethodName + ": " + nameof(OnExampleCompleted), jsonArg), ex);
            }

            sink.ExampleCompleted(example);
        }

        static object CreateController(Assembly nspecLibraryAssembly)
        {
            try
            {
                var typeInfo = nspecLibraryAssembly.DefinedTypes.Single(t => t.FullName == controllerTypeName);

                object controller = Activator.CreateInstance(typeInfo.AsType());

                return controller;
            }
            catch (Exception ex)
            {
                throw new DotNetTestNSpecException(unknownDriverErrorMessage.With(controllerTypeName), ex);
            }
        }

        static object InvokeMethod(object controller, string methodName, params object[] args)
        {
            var controllerType = controller.GetType();

            var methodInfo = controllerType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

            if (methodInfo == null)
            {
                throw new DotNetTestNSpecException(unknownMethodErrorMessage.With(methodName));
            }

            object result = methodInfo.Invoke(controller, args);

            return result;
        }

        readonly object controller;

        const string controllerTypeName = "NSpec.Api.Controller";

        const string runBatchMethodName = "RunBatch";
        const string listMethodName = "List";
        const string runInteractiveMethodName = "RunInteractive";

        const string unknownDriverErrorMessage =
            "Could not find known driver ({0}) in referenced NSpec assembly: " +
            "please double check version compatibility between this runner and referenced NSpec library.";
        const string unknownMethodErrorMessage =
            "Could not find known method ({0}) in referenced NSpec assembly: " +
            "please double check version compatibility between this runner and referenced NSpec library.";
        const string unknownResultErrorMessage =
            "Could not convert serialized result from known method ({0}) in referenced NSpec assembly: " +
            "please double check version compatibility between this runner and referenced NSpec library." +
            "Result: {1}.";
        const string unknownArgumentErrorMessage =
            "Could not convert serialized argument from known callback ({0}) in referenced NSpec assembly: " +
            "please double check version compatibility between this runner and referenced NSpec library." +
            "Argument: {1}.";
    }
}
