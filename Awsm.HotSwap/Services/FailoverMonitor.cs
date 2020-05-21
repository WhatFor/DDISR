namespace Awsm.HotSwap
{
    public class FailoverMonitor<TInterface> : IFailoverMonitor<TInterface>
        where TInterface : class
    {
        private readonly HotSwapInternalConfiguration<TInterface> config;
        
        public FailoverMonitor(HotSwapInternalConfiguration<TInterface> config)
        {
            this.config = config;
        }
        
        public void RecordFailure<TImpl>()
            where TImpl : TInterface
        {
            config.Flags.AutoRecoveryConfiguration.AddFailure(typeof(TImpl));
        }
    }
}