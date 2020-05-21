using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Awsm.HotSwap.Middleware
{
    public class ManualManagementMiddleware
    {
        private readonly RequestDelegate next;

        private IServiceProvider serviceProvider;
        
        private readonly HotSwapConfiguration config;
        
        private readonly ServiceSummaryProvider summaryProvider = new ServiceSummaryProvider();

        private static readonly Regex routeServiceRegex = new Regex($"([0-9]*)$");
        
        private static readonly Regex routeActionRegex = new Regex($"(SWITCH)", RegexOptions.IgnoreCase); 

        public ManualManagementMiddleware(RequestDelegate next, HotSwapConfiguration config)
        {
            this.next = next;
            this.config = config;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            var path = context.Request.Path;

            if (path.StartsWithSegments(config.Endpoint))
            {
                var route = CheckRoute(path);

                if (route.Error != null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync(route.Error);
                    return;
                }

                switch (route.Action)
                {
                    case RouteAction.SUMMARY:
                        context.Response.StatusCode = 200;
                        context.Response.ContentType = "application/json";
                        var summary = summaryProvider.GetSummary(serviceProvider);
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(summary));
                        return;

                    case RouteAction.SWITCH:
                        var result = summaryProvider.SwitchActiveService(serviceProvider, route.ServiceId);
                        if (result)
                        {
                            context.Response.StatusCode = 200;
                            return;
                        }
                        else
                        {
                            context.Response.StatusCode = 500;
                            return;
                        }
                }
            }

            await next(context);
        }


        private RouteParams CheckRoute(PathString path)
        {
            var routeParams = new RouteParams();

            if (path.Value == config.Endpoint)
            {
                routeParams.Action = RouteAction.SUMMARY;
                return routeParams;
            }

            if (routeActionRegex.IsMatch(path.Value) == false)
            {
                routeParams.Error = "Route not found.";
            }

            var routeResult = routeActionRegex.Match(path.Value).Groups[0].Value;

            if (string.IsNullOrWhiteSpace(routeResult))
            {
                routeParams.Error = "No action found.";
                return routeParams;
            }

            routeParams.Action = RouteAction.SWITCH;
            var serviceResultStr = routeServiceRegex.Match(path.Value).Groups[0].Value;
            var serviceResultSuccess = int.TryParse(serviceResultStr, out var serviceResult);

            if (!serviceResultSuccess)
            {
                routeParams.Error = "ID is not a valid value.";
                return routeParams;
            }

            if (ServiceExists(serviceResult) == false)
            {
                routeParams.Error = "Service not found.";
                return routeParams;
            }

            routeParams.ServiceId = serviceResult;
            return routeParams;
        }

        /// <summary>
        /// Check if a service has been registered.
        /// </summary>
        private bool ServiceExists(int serviceId)
        {
            var serviceNames = summaryProvider.GetServiceIds(serviceProvider);
            return serviceNames.Contains(serviceId);
        }
    }

    public class RouteParams
    {
        public RouteAction Action { get; set; }
        
        public int ServiceId { get; set; }

        public string Error { get; set; }
    }
    
    public enum RouteAction
    {
        SUMMARY,
        SWITCH,
    }
}