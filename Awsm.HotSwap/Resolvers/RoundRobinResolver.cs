using System.Linq;

namespace Awsm.HotSwap.Resolvers
{
    internal class RoundRobinResolver : IServiceResolver
    {
        public ImplementationTypeDescriptor Resolve<T>(HotSwapInternalConfiguration<T> config)
            where T : class
        {
            var impl = config.ImplementationTypes.ElementAt(config.Flags.RoundRobinConfiguration.IndexCurrent);

            if (config.Flags.RoundRobinConfiguration.IsAtMax)
            {
                config.Flags.RoundRobinConfiguration.IndexCurrent = 0;
            }
            else
            {
                config.Flags.RoundRobinConfiguration.IndexCurrent++;
            }

            return impl;
        }
    }
}