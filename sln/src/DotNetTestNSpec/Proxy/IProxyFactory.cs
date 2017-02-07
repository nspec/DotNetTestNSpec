namespace DotNetTestNSpec.Proxy
{
    public interface IProxyFactory
    {
        IControllerProxy Create(string testAssemblyPath);
    }
}
