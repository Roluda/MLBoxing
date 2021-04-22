using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML {
    public abstract class Terminater : ScriptableObject {
        public abstract void AddTerminationListeners(ModularAgent agent);
    }
}
