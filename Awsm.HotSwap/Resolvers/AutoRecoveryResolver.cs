using System.Linq;

namespace Awsm.HotSwap.Resolvers
{
    internal class AutoRecoveryResolver : IServiceResolver
    {
        public ImplementationTypeDescriptor Resolve<T>(HotSwapInternalConfiguration<T> config) where T : class
        {
            // If none are active/ default, set one
            if (!config.ImplementationTypes.Any(x => x.IsActive))
            {
                config.ImplementationTypes.OrderBy(x => x.Priority).FirstOrDefault().IsActive = true;
            }
            
            if (config.Flags.AutoRecoveryConfiguration.IsStable(config.ActiveImplementation.Type))
            {
                // Active is stable...
                return config.ActiveImplementation;
            }
            else
            {
                // Active is not stable...
                config.Flags.AutoRecoveryConfiguration.FailedServices.Add(config.ActiveImplementation.Type);
                
                // Find one that is!
                var next = config.ImplementationTypes
                    // Where not excluded from failover,    
                    .Where(x => !x.ExcludeFromFailover)
                    // Where not failed already,
                    .Where(x => !config.Flags.AutoRecoveryConfiguration.FailedServices.Contains(x.Type))
                    // and then Ordered by priority.
                    .OrderBy(x => x.Priority)
                    .FirstOrDefault();

                config.SetActive(next);
                return next;
            }
        }
    }
}