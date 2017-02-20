namespace DotNetTestNSpec.Domain.Library
{
    public interface IExecutionSink
    {
        void ExampleStarted(DiscoveredExample example);

        void ExampleCompleted(ExecutedExample example);
    }
}
