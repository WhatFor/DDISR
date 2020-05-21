namespace Awsm.HotSwap.Resolvers
{
    internal class ServiceResolverFactory
    {
        internal IServiceResolver GetServiceResolver(ConfigurationContainer flags)
        {
            if (flags.RoundRobinEnabled)
            {
                return new RoundRobinResolver();
            }

            if (flags.AutoRecoveryEnabled)
            {
                return new AutoRecoveryResolver();
            }

            return new DefaultResolver();
        }
    }
}