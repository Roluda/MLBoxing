using MLBoxing.Ragdoll;
using UnityEngine;

namespace MLBoxing.ML.Rewards {
    [CreateAssetMenu(fileName = "R_Posture_New", menuName = "ML/Rewards/Posture")]
    public class PostureReward : Reward {
        [SerializeField]
        float headDistanceToLineMultiplier = 0.005f;
        [SerializeField]
        float chestDistanceToLineMultiplier = 0.005f;


        public override void AddRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate += CalculatePostureRewards;
        }

        private void CalculatePostureRewards(ModularAgent agent) {
            float headDistance = DistanceToLine(agent.ragdoll.root.position, agent.ragdoll.GetLimb(LimbType.Head).position);
            float chestDistance = DistanceToLine(agent.ragdoll.root.position, agent.ragdoll.GetLimb(LimbType.Torso).position);
            if (headDistanceToLineMultiplier != 0) {
                agent.AddReward(headDistance * headDistanceToLineMultiplier, nameof(headDistanceToLineMultiplier), asScore);
            }
            if (chestDistanceToLineMultiplier != 0) {
                agent.AddReward(chestDistance * chestDistanceToLineMultiplier, nameof(chestDistanceToLineMultiplier), asScore);
            }
        }

        public override void RemoveRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate -= CalculatePostureRewards;
        }

        float DistanceToLine(Vector3 postureContext, Vector3 point) {
            return Vector3.Cross(Vector3.up, point - postureContext).magnitude;
        }
    }
}