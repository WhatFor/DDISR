using System.Linq;

namespace Awsm.HotSwap.Tests
{
    public class ReverseStringService : IStringService
    {
        public string FormatString(string input)
        {
            return string.Concat(input.Reverse());
        }
    }
}