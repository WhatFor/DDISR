namespace Awsm.HotSwap
{
    public class HotSwapConfiguration
    {
        public HotSwapConfiguration() { }

        public HotSwapConfiguration(string endpoint)
        {
            Endpoint = endpoint;
        }
        
        public string Endpoint { get; set; }
    }
}