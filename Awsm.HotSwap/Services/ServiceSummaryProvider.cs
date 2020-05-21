using System;
using System.Collections.Generic;

namespace Awsm.HotSwap
{
    public class ServiceSummaryProvider
    {
        // TODO: Flesh out with functionality
        public string GetSummary(IServiceProvider serviceProvider)
        {
            return default;
        }

        public bool SwitchActiveService(IServiceProvider serviceProvider, int serviceId)
        {
            return true;
        }

        public IEnumerable<int> GetServiceIds(IServiceProvider serviceProvider)
        {
            return default;
        }
    }
}