using System;
using System.Collections.Generic;
using System.Linq;

namespace Awsm.HotSwap
{
    public class AutoRecoveryConfiguration
    {
        public TimeSpan ErrorWindow { get; set; }
        
        public uint ErrorCount { get; set; }

        internal Dictionary<Type, List<DateTimeOffset>> TypeFailures { get; set; }
            = new Dictionary<Type, List<DateTimeOffset>>();

        internal List<Type> FailedServices { get; set; }
            = new List<Type>();

        internal void AddFailure(Type type)
        {
            if (TypeFailures.ContainsKey(type))
            {
                TypeFailures[type].Add(DateTimeOffset.Now);
            }
            else
            {
                TypeFailures[type] = new List<DateTimeOffset> { DateTimeOffset.Now };
            }

            // Trim the expired events
            TypeFailures[type].RemoveAll(x => x < DateTimeOffset.Now.Subtract(ErrorWindow));
        }

        internal bool IsStable(Type type)
        {
            if (TypeFailures.ContainsKey(type))
                return TypeFailures[type].Count < ErrorCount;

            return true;
        }

        internal void SetIsStable(Type type)
        {
            if (FailedServices.Any(x => x == type))
                FailedServices.Remove(type);
        }
    }
}