namespace DotNetTestNSpec.Proxy
{
    public interface IRunSink
    {
        void ExampleStarted(DiscoveredExample example);

        void ExampleCompleted(ExecutedExample example);
    }
}
