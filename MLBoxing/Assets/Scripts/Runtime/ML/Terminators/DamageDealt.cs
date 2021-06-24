using UnityEngine;

namespace MLBoxing.ML.Terminators {
    [CreateAssetMenu(fileName = "T_DamageDealt_New", menuName = "ML/Terminaters/DamageDealt")]
    public class DamageDealt : Terminator {
        [SerializeField]
        float damageDealtThreshold = 10;

        public override void AddTerminationListeners(ModularAgent agent) {
            agent.onFixedUpdate += CheckDamageDealt;
        }

        public override void RemoveTerminationListeners(ModularAgent agent) {
            agent.onFixedUpdate -= CheckDamageDealt;
        }

        void CheckDamageDealt(ModularAgent agent) {
            if (agent.ragdoll.totalDamageDealt > damageDealtThreshold) {
                agent.Terminate();
            }
        }
    }
}

