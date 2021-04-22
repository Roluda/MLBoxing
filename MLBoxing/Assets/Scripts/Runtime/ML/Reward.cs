using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML {
    public abstract class Reward : ScriptableObject {
        public abstract void AddRewardListeners(ModularAgent agent);
    }
}
