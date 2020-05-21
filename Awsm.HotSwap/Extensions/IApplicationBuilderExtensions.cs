using System;
using Awsm.HotSwap.Middleware;
using Microsoft.AspNetCore.Builder;

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

            if (!conf.Endpoint.StartsWith("/"))
                throw new ArgumentException("HotSwapServices Endpoint must begin with '/'.");
            
            // Register middleware
            app.UseMiddleware<ManualManagementMiddleware>(conf);
            
            return app;
        }
    }
}