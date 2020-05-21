using System.Linq;

namespace Awsm.HotSwap.Resolvers
{
    internal class DefaultResolver : IServiceResolver
    {
        public ImplementationTypeDescriptor Resolve<T>(HotSwapInternalConfiguration<T> config)
            where T : class
        {
            // If we don't have a default service, set one
            if (config.ImplementationTypes.All(x => !x.IsActive))
            {
                config.ImplementationTypes.First().IsActive = true;
                config.ImplementationTypes.First().IsDefault = true;
            }
            
            return config.ImplementationTypes.Single(x => x.IsActive);
        }
    }
}