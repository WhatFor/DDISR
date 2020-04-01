using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Awsm.HotSwap
{
    internal interface IHotSwapInternalConfiguration
    {
        void ConfigureServices<TImpl>(IServiceCollection services);
    }

    internal class HotSwapInternalConfiguration<TInterface> : IHotSwapInternalConfiguration
        where TInterface : class
    {
        internal ScopeLevel Level { get; }
        internal Type InterfaceType { get; }
        internal ICollection<ImplementationTypeDescriptor> ImplementationTypes { get; } = new List<ImplementationTypeDescriptor>();

        public HotSwapInternalConfiguration(IServiceCollection services, ScopeLevel level)
        {
            Level = level;
            InterfaceType = typeof(TInterface);
            services.AddSingleton<HotSwapServiceFactory<TInterface>>();
        }
        
        public void AddImplementation<TImplementation>(bool isDefault) =>
            ImplementationTypes.Add(
                new ImplementationTypeDescriptor
                    { Type = typeof(TImplementation),
                        IsDefault = isDefault,
                        IsActive = isDefault,
                    });

        public void ConfigureServices<TImpl>(IServiceCollection services)
        {
            var type = typeof(TImpl);
            
            switch (Level)
            {
                case ScopeLevel.Scoped:
                    services.AddScoped(type);
                    services.AddScoped(o => o
                        .GetRequiredService<HotSwapServiceFactory<TInterface>>()
                        .Resolve());
                    break;
                case ScopeLevel.Transient:
                    services.AddTransient(type);
                    services.AddTransient(o => o
                        .GetRequiredService<HotSwapServiceFactory<TInterface>>()
                        .Resolve());
                    break;
                case ScopeLevel.Singleton:
                    services.AddSingleton(type);
                    services.AddSingleton(o => o
                        .GetRequiredService<HotSwapServiceFactory<TInterface>>()
                        .Resolve());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}