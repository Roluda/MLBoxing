using MLBoxing.Ragdoll;
using System.Linq;
using UnityEngine;

namespace MLBoxing.ML.Rewards {
    [CreateAssetMenu(fileName="R_EnergyUse_New", menuName ="ML/Rewards/EnergyUse")]
    public class EnergyUseReward : Reward {
        [SerializeField, Range(-1, 1)]
        float angularEnergyScale = -0.001f;
        [SerializeField, Range(-1, 1)]
        float linearEnergyScale = -0.001f;
        [SerializeField, Range(-1, 1)]
        float torqueScale = -0.001f;
        [SerializeField, Range(-1, 1)]
        float opponentAngularEnergyScale = 0.001f;
        [SerializeField, Range(-1, 1)]
        float opponentLinearEnergyScale = 0.001f;
        [SerializeField, Range(-1, 1)]
        float enemyTorqueScale = 0.001f;

        [SerializeField]
        LimbType relevantLimbs = default;
        [SerializeField]
        JointType relevantJoints = default;


        public override void AddRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate += AddEnergyRewards;
        }

        public override void RemoveRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate -= AddEnergyRewards;
        }

        private void AddEnergyRewards(ModularAgent agent) {
            if(angularEnergyScale != 0) {
                float angularEnergy = agent.ragdoll.FilterLimbs(relevantLimbs).Sum(limb => limb.angularEnergy);
                agent.AddReward(angularEnergy * angularEnergyScale, nameof(angularEnergyScale), asScore);
            }
            if(linearEnergyScale != 0) {
                float linearEnergy = agent.ragdoll.FilterLimbs(relevantLimbs).Sum(limb => limb.linearEnergy);
                agent.AddReward(linearEnergy * linearEnergyScale, nameof(linearEnergyScale), asScore);

            }
            if (torqueScale != 0) {
                float torque = agent.ragdoll.FilterJoints(relevantJoints).Sum(joint => joint.currentTorque);
                agent.AddReward(torque * torqueScale, nameof(torqueScale), asScore);
            }
            if (opponentAngularEnergyScale != 0) {
                float angularEnergy = agent.opponent.ragdoll.FilterLimbs(relevantLimbs).Sum(limb => limb.angularEnergy);
                agent.AddReward(angularEnergy * opponentAngularEnergyScale, nameof(opponentAngularEnergyScale), asScore);
            }
            if (opponentLinearEnergyScale != 0) {
                float linearEnergy = agent.opponent.ragdoll.FilterLimbs(relevantLimbs).Sum(limb => limb.linearEnergy);
                agent.AddReward(linearEnergy * opponentLinearEnergyScale, nameof(opponentLinearEnergyScale), asScore);
            }
            if (enemyTorqueScale != 0) {
                float torque = agent.opponent.ragdoll.FilterJoints(relevantJoints).Sum(joint => joint.currentTorque);
                agent.AddReward(torque * enemyTorqueScale, nameof(enemyTorqueScale), asScore);
            }
        }
    }
}
