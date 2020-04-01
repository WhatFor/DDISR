using System;

namespace Awsm.HotSwap
{
    internal class ImplementationTypeDescriptor
    {
        internal Type Type { get; set; }
        
        internal bool IsDefault { get; set; }
        
        internal bool IsActive { get; set; }
    }
}