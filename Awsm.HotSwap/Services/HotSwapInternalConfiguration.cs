using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Awsm.HotSwap
{
    public class HotSwapInternalConfiguration<TInterface> : IHotSwapInternalConfiguration
        where TInterface : class
    {
        internal ScopeLevel Level { get; }
        internal Type InterfaceType { get; }
        internal ConfigurationContainer Flags { get; } = new ConfigurationContainer();
        internal ICollection<ImplementationTypeDescriptor> ImplementationTypes { get; } = new List<ImplementationTypeDescriptor>();

        internal ImplementationTypeDescriptor ActiveImplementation =>
            ImplementationTypes.Single(x => x.IsActive);

        public HotSwapInternalConfiguration(IServiceCollection services, ScopeLevel level)
        {
            Level = level;
            InterfaceType = typeof(TInterface);
            services.AddSingleton<HotSwapServiceFactory<TInterface>>();
        }
        
        public void AddImplementation<TImplementation>(ImplementationConfiguration config) =>
            ImplementationTypes.Add(
                new ImplementationTypeDescriptor
                    { Type = typeof(TImplementation),
                        IsDefault = false,
                        IsActive = false,
                        Priority = config.FailoverPriority,
                        ExcludeFromFailover = config.ExcludeFromFailover,
                    });
        
        public void AddDefaultImplementation<TImplementation>(ImplementationConfiguration config) =>
            ImplementationTypes.Add(
                new ImplementationTypeDescriptor
                    { Type = typeof(TImplementation),
                        IsDefault = true,
                        IsActive = true,
                        Priority = config.FailoverPriority,
                        ExcludeFromFailover = config.ExcludeFromFailover,
                    });

        public void WithAutoRecovery(AutoRecoveryConfiguration config)
        {
            Flags.AutoRecoveryEnabled = true;
            Flags.AutoRecoveryConfiguration = config;
        }
        
        public void WithRoundRobinSelection(RoundRobinConfiguration config)
        {
            Flags.RoundRobinEnabled = true;
            Flags.RoundRobinConfiguration = config;
            Flags.RoundRobinConfiguration.IndexCurrent = 0;
            Flags.RoundRobinConfiguration.IndexMax = ImplementationTypes.Count - 1;
        }

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

        internal void SetActive(ImplementationTypeDescriptor implementationType)
        {
            foreach (var type in ImplementationTypes)
            {
                type.IsActive = false;
            }

            ImplementationTypes.Single(x => x.Type == implementationType.Type).IsActive = true;
        }
    }
}