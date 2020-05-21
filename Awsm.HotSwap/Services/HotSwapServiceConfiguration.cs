using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Awsm.HotSwap
{
    public class HotSwapServiceConfiguration<TInterface>
        where TInterface : class
    {
        private readonly IServiceCollection services;
        private readonly HotSwapInternalConfiguration<TInterface> config;
        
        internal HotSwapServiceConfiguration(
            IServiceCollection services,
            ScopeLevel level)
        {
            this.services = services;
            this.config = new HotSwapInternalConfiguration<TInterface>(services, level);
        }

        public HotSwapServiceConfiguration<TInterface> AddImplementation<TImplementation>(
            Action<ImplementationConfiguration> configAction = default)
                where TImplementation : TInterface
        {
            var implConfig = new ImplementationConfiguration();
            configAction?.Invoke(implConfig);
            
            config.AddImplementation<TImplementation>(implConfig);
            return InternalAddImplementation<TImplementation>();
        }
        
        public HotSwapServiceConfiguration<TInterface> AddDefaultImplementation<TImplementation>(
            Action<ImplementationConfiguration> configAction = default)
                where TImplementation : TInterface
        {
            var implConfig = new ImplementationConfiguration();
            configAction?.Invoke(implConfig);
            
            config.AddDefaultImplementation<TImplementation>(implConfig);
            return InternalAddImplementation<TImplementation>();
        }

        public HotSwapServiceConfiguration<TInterface> WithAutoRecovery(
            Action<AutoRecoveryConfiguration> configAction = default)
        {
            var autoRecoveryConfig = new AutoRecoveryConfiguration();
            configAction?.Invoke(autoRecoveryConfig);
            config.WithAutoRecovery(autoRecoveryConfig);

            services.AddScoped<IFailoverMonitor<TInterface>, FailoverMonitor<TInterface>>();
            
            return this;
        }
        
        public HotSwapServiceConfiguration<TInterface> WithRoundRobinSelection(
            Action<RoundRobinConfiguration> configAction = default)
        {
            var autoRecoveryConfig = new RoundRobinConfiguration();
            configAction?.Invoke(autoRecoveryConfig);
            config.WithRoundRobinSelection(autoRecoveryConfig);
            
            return this;
        }

        private HotSwapServiceConfiguration<TInterface> InternalAddImplementation<TImplementation>()
            where TImplementation : TInterface
        {
            config.ConfigureServices<TImplementation>(services);
            services.AddSingleton(config);
            return this;
        }
    }
}