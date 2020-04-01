using Microsoft.Extensions.DependencyInjection;

namespace Awsm.HotSwap
{
    public class HotSwapServiceConfiguration<TInterface>
        where TInterface : class
    {
        private readonly IServiceCollection services;
        private readonly HotSwapInternalConfiguration<TInterface> config;
        
        internal HotSwapServiceConfiguration(IServiceCollection services, ScopeLevel level)
        {
            this.services = services;
            this.config = new HotSwapInternalConfiguration<TInterface>(services, level);
        }

        public HotSwapServiceConfiguration<TInterface> AddImplementation<TImplementation>(bool defaultImplementation = false)
            where TImplementation : TInterface
        {
            config.AddImplementation<TImplementation>(defaultImplementation);
            config.ConfigureServices<TImplementation>(services);
            
            services.AddSingleton(config);
            
            return this;
        }
    }
}