namespace DotNetTestNSpec.Domain.Library
{
    public interface IProxyFactory
    {
        IControllerProxy Create(string testAssemblyPath);
    }
}
