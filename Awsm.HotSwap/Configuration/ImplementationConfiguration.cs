namespace Awsm.HotSwap
{
    public class ImplementationConfiguration
    {
        public int Id { get; set; }
        
        public int FailoverPriority { get; set; }
        
        public bool ExcludeFromFailover { get; set; }
    }
}