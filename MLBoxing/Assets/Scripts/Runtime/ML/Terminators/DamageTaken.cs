using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML.Terminators {
    [CreateAssetMenu(fileName = "T_DamageTaken_New", menuName = "ML/Terminaters/DamageTaken")]
    public class DamageTaken : Terminator {
        [SerializeField]
        float damageTakenThreshold = 10;

        public override void AddTerminationListeners(ModularAgent agent) {
            agent.onFixedUpdate += CheckDamageTaken;
        }

        public override void RemoveTerminationListeners(ModularAgent agent) {
            agent.onFixedUpdate -= CheckDamageTaken;
        }

        void CheckDamageTaken(ModularAgent agent) {
            if(agent.ragdoll.totalDamageTaken > damageTakenThreshold) {
                agent.Terminate();
            }
        }
    }
}
