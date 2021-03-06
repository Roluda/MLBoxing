using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML {
    public abstract class Reward : ScriptableObject {
        [SerializeField]
        protected bool asScore = false;
        public abstract void AddRewardListeners(ModularAgent agent);
        public abstract void RemoveRewardListeners(ModularAgent agent);
    }
}
