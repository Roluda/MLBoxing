using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML {
    public abstract class Terminator : ScriptableObject {
        public abstract void AddTerminationListeners(ModularAgent agent);
        public abstract void RemoveTerminationListeners(ModularAgent agent);
    }
}
