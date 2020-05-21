namespace Awsm.HotSwap
{
    internal class ConfigurationContainer
    {
        internal bool AutoRecoveryEnabled { get; set; }
        
        internal bool RoundRobinEnabled { get; set; }
        
        internal AutoRecoveryConfiguration AutoRecoveryConfiguration { get; set; }
        
        internal RoundRobinConfiguration RoundRobinConfiguration { get; set; }
    }
}