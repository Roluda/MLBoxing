using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MLBoxing.ML {
    public abstract class Score : ScriptableObject {
        public abstract void AddScoreListeners(ModularAgent agent);
        public abstract void RemoveScoreListener(ModularAgent agent);
    }
}