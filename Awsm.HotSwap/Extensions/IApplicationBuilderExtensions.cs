using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Awsm.HotSwap
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHotSwapServices(
            this IApplicationBuilder app,
            Action<HotSwapConfiguration> configureAction = null)
        {
            // Collect Config
            var conf = new HotSwapConfiguration("/api/hot-swap");
            configureAction?.Invoke(conf);
            
            return app;
        }
    }
}