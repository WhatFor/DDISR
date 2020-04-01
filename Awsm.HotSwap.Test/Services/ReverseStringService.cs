using System.Linq;

namespace Awsm.HotSwap.Test
{
    public class ReverseStringService : IStringService
    {
        public string FormatString(string input) => string.Concat(input.Reverse());
    }
}