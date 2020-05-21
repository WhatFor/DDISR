namespace Awsm.HotSwap
{
    public class RoundRobinConfiguration
    {
        /// <summary>
        /// The current RoundRobin counter value.
        /// </summary>
        internal int IndexCurrent { get; set; }
        
        /// <summary>
        /// A cap on the RoundRobin counter.
        /// </summary>
        internal int IndexMax { get; set; }

        internal bool IsAtMax => IndexCurrent == IndexMax;
    }
}