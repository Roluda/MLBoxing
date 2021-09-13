using MLBoxing.Ragdoll;
using System.Linq;
using UnityEngine;

namespace MLBoxing.ML.Rewards {
    [CreateAssetMenu(fileName="R_EnergyUse_New", menuName ="ML/Rewards/EnergyUse")]
    public class EnergyUseReward : Reward {
        [SerializeField, Range(-1, 1)]
        float torqueScale = -0.001f;
        [SerializeField, Range(-1, 1)]
        float enemyTorqueScale = 0.001f;
        [SerializeField, Range(-1, 1)]
        float deltaTorqueScale = -0.001f;
        [SerializeField, Range(-1, 1)]
        float enemyDeltaTorqueScale = -0.001f;



        [SerializeField]
        JointType relevantJoints = default;


        public override void AddRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate += AddEnergyRewards;
        }

        public override void RemoveRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate -= AddEnergyRewards;
        }

        private void AddEnergyRewards(ModularAgent agent) {
            if (torqueScale != 0) {
                float torque = agent.ragdoll.FilterJoints(relevantJoints).Sum(joint => joint.currentTorque);
                agent.AddReward(torque * torqueScale, nameof(torqueScale), asScore);
            }
            if (enemyTorqueScale != 0) {
                float torque = agent.opponent.ragdoll.FilterJoints(relevantJoints).Sum(joint => joint.currentTorque);
                agent.AddReward(torque * enemyTorqueScale, nameof(enemyTorqueScale), asScore);
            }
            if (deltaTorqueScale != 0) {
                float torque = agent.ragdoll.FilterJoints(relevantJoints).Sum(joint => Mathf.Pow(joint.deltaTorque, 2));
                agent.AddReward(torque * deltaTorqueScale, nameof(deltaTorqueScale), asScore);
            }
            if (enemyDeltaTorqueScale != 0) {
                float torque = agent.ragdoll.FilterJoints(relevantJoints).Sum(joint => Mathf.Pow(joint.deltaTorque, 2));
                agent.AddReward(torque * enemyDeltaTorqueScale, nameof(enemyDeltaTorqueScale), asScore);
            }
        }
    }
}
