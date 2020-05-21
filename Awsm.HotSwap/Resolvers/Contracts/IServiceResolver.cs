namespace Awsm.HotSwap.Resolvers
{
    internal interface IServiceResolver
    {
        ImplementationTypeDescriptor Resolve<T>(HotSwapInternalConfiguration<T> config)
            where T : class;
    }
}