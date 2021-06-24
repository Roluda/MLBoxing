using UnityEngine;

namespace MLBoxing.ML {
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
            float headDistance = DistanceToLine(agent.character.position, agent.character.head.transform.position);
            float chestDistance = DistanceToLine(agent.character.position, agent.character.chest.transform.position);
            agent.AddReward(headDistance * headDistanceToLineMultiplier , nameof(headDistanceToLineMultiplier));
            agent.AddReward(chestDistance * chestDistanceToLineMultiplier , nameof(chestDistanceToLineMultiplier));
            Debug.Log($"Posture: {headDistance* headDistanceToLineMultiplier + chestDistance* chestDistanceToLineMultiplier}");
        }

        public override void RemoveRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate -= CalculatePostureRewards;
        }

        float DistanceToLine(Vector3 postureContext, Vector3 point) {
            return Vector3.Cross(Vector3.up, point - postureContext).magnitude;
        }
    }
}