using System;
using System.Linq;

namespace Awsm.HotSwap.Test
{
    public class ReverseStringService : IStringService
    {
        private readonly IFailoverMonitor<IStringService> failMon;

        public ReverseStringService(IFailoverMonitor<IStringService> failMon)
        {
            this.failMon = failMon;
        }

        public string FormatString(string input)
        {
            try
            {
                throw new Exception();
                string.Concat(input.Reverse());
            }
            catch
            {
                failMon.RecordFailure<ReverseStringService>();
            }

            return "Something went wrong.";
        }
    }
}