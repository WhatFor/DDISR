using Microsoft.Extensions.DependencyInjection;

namespace Awsm.HotSwap
{
    internal interface IHotSwapInternalConfiguration
    {
        void ConfigureServices<TImpl>(IServiceCollection services);
    }
}