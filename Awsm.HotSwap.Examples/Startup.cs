using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Awsm.HotSwap.Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScopedHotSwapService<IStringService>()
                .AddImplementation<ReverseStringService>(o =>
                {
                    o.Id = 1;
                    o.FailoverPriority = 0;
                })
                .AddImplementation<UppercaseStringService>(o =>
                {
                    o.Id = 2;
                    o.ExcludeFromFailover = true;
                    o.FailoverPriority = 1;
                })
                .AddImplementation<TruncatedStringService>(o =>
                {
                    o.Id = 3;
                    o.FailoverPriority = 2;
                })
                // .WithAutoRecovery(o =>
                // {
                //     o.ErrorCount = 3;
                //     o.ErrorWindow = TimeSpan.FromMinutes(1);
                // });
                .WithRoundRobinSelection();
            
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHotSwapServices(o =>
            {
                o.Endpoint = "/services";
            });
            
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}