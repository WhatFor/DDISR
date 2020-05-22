namespace Awsm.HotSwap.Tests
{
    public class UppercaseStringService : IStringService
    {
        public string FormatString(string input) => input.ToUpper();
    }
}