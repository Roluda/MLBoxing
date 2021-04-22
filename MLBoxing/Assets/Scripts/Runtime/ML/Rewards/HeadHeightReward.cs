using UnityEngine;

namespace MLBoxing.ML {
    [CreateAssetMenu(fileName="R_HeadHeight_New", menuName ="ML/Rewards/HeadHeight")]
    public class HeadHeightReward : Reward {
        [SerializeField, Range(-1, 1), Tooltip("Reward each step if above threshold")]
        float headAboveReward = 0.01f;
        [SerializeField, Range(0, 1), Tooltip("Threshold for height reward")]
        float headAboveThreshold = 0.5f;
        [SerializeField, Range(-1, 1), Tooltip("Multipier for normalized height")]
        float headHeightMultiplier = 0;


        public override void AddRewardListeners(ModularAgent agent) {
            agent.onEpisodeStep += CheckHeadAboveReward;
            agent.onEpisodeStep += AddMultiplicativeHeadHeightReward;
        }

        private void AddMultiplicativeHeadHeightReward(ModularAgent agent) {
            if (headHeightMultiplier != 0) {
                agent.AddReward(agent.character.head.transform.position.y / agent.character.height * headHeightMultiplier);
            }
        }

        private void CheckHeadAboveReward(ModularAgent agent) {
            if (agent.character.head.transform.position.y / agent.character.height > headAboveThreshold) {
                agent.AddReward(headAboveReward);
            }
        }
    }
}
