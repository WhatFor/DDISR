using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Awsm.HotSwap
{
    internal class HotSwapServiceFactory<TInterface>
        where TInterface : class
    {
        private readonly HotSwapInternalConfiguration<TInterface> config;
        private readonly IServiceProvider serviceProvider;
        public HotSwapServiceFactory(
            HotSwapInternalConfiguration<TInterface> config,
            IServiceProvider serviceProvider)
        {
            this.config = config;
            this.serviceProvider = serviceProvider;
        }
        
        public TInterface Resolve()
        {
            var activeType = config.ImplementationTypes.Single(x => x.IsActive);
            return serviceProvider.CreateScope().ServiceProvider.GetRequiredService(activeType.Type) as TInterface;
        }
    }
}