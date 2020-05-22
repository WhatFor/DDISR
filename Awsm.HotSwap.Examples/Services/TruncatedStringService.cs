using System.Linq;

namespace Awsm.HotSwap.Test
{
    public class TruncatedStringService : IStringService
    {
        public string FormatString(string input) =>
            input.Length > 5 ? input.Substring(0, 5) + "..." : input;
    }
}