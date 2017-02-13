namespace DotNetTestNSpec.Proxy
{
    public interface IExecutionSink
    {
        void ExampleStarted(DiscoveredExample example);

        void ExampleCompleted(ExecutedExample example);
    }
}
