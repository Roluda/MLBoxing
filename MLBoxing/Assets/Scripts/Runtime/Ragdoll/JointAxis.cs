using System;

namespace MLBoxing.Ragdoll {
    [Flags]
    public enum DOF {
        X = 1<<0,
        Y = 1<<1,
        Z = 1<<2
    }
}
