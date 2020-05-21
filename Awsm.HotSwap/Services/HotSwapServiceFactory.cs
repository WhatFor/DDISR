using System;
using System.Linq;
using Awsm.HotSwap.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Awsm.HotSwap
{
    internal class HotSwapServiceFactory<TInterface>
        where TInterface : class
    {
        private readonly HotSwapInternalConfiguration<TInterface> config;
        private readonly ServiceResolverFactory resolverFactory;
        private readonly IServiceProvider serviceProvider;
        
        public HotSwapServiceFactory(
            HotSwapInternalConfiguration<TInterface> config,
            ServiceResolverFactory resolverFactory,
            IServiceProvider serviceProvider)
        {
            this.config = config;
            this.resolverFactory = resolverFactory;
            this.serviceProvider = serviceProvider;
        }
        
        public TInterface Resolve()
        {
            var factory = resolverFactory.GetServiceResolver(config.Flags);
            var implementationDescriptor = factory.Resolve(config);
            return serviceProvider.CreateScope().ServiceProvider.GetRequiredService(implementationDescriptor.Type) as TInterface;
        }
    }
}