namespace Awsm.HotSwap
{
    public interface IFailoverMonitor<TInterface>
        where TInterface : class
    {
        void RecordFailure<TImpl>()
            where TImpl : TInterface;
    }
}