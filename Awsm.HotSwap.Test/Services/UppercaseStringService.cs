namespace Awsm.HotSwap.Test
{
    public class UppercaseStringService : IStringService
    {
        public string FormatString(string input) => input.ToUpper();
    }
}