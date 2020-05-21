using Awsm.HotSwap.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace Awsm.HotSwap
{
    public static class IServiceCollectionExtensions
    {
        public static HotSwapServiceConfiguration<TInterface> AddSingletonHotSwapService<TInterface>(this IServiceCollection services)
            where TInterface : class
        {
            services.AddSingleton<ServiceResolverFactory>();
            return new HotSwapServiceConfiguration<TInterface>(services, ScopeLevel.Singleton);
        }
        
        public static HotSwapServiceConfiguration<TInterface> AddTransientHotSwapService<TInterface>(this IServiceCollection services)
            where TInterface : class
        {
            services.AddSingleton<ServiceResolverFactory>();
            return new HotSwapServiceConfiguration<TInterface>(services, ScopeLevel.Transient);
        }
        
        public static HotSwapServiceConfiguration<TInterface> AddScopedHotSwapService<TInterface>(this IServiceCollection services)
            where TInterface : class
        {
            services.AddSingleton<ServiceResolverFactory>();
            return new HotSwapServiceConfiguration<TInterface>(services, ScopeLevel.Scoped);
        }
    }
}