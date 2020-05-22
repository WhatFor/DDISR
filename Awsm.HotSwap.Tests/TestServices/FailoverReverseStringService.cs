using System;
using System.Linq;

namespace Awsm.HotSwap.Tests
{
    public class FailoverReverseStringService : IStringService
    {
        private readonly IFailoverMonitor<IStringService> failMon;

        public FailoverReverseStringService(IFailoverMonitor<IStringService> failMon)
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
                failMon.RecordFailure<FailoverReverseStringService>();
            }

            return "Something went wrong.";
        }
    }
}