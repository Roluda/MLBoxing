using MLBoxing.Ragdoll;
using System.Linq;
using UnityEngine;

namespace MLBoxing.ML.Rewards {
    [CreateAssetMenu(fileName="R_HeadHeight_New", menuName ="ML/Rewards/HeadHeight")]
    public class HeadHeightReward : Reward {
        [SerializeField, Range(-1, 1), Tooltip("Reward each step if above threshold")]
        float headAboveReward = 0.01f;
        [SerializeField, Range(0, 1), Tooltip("Threshold for height reward")]
        float headAboveThreshold = 0.5f;
        [SerializeField, Range(-1, 1), Tooltip("Multipier for normalized height")]
        float headHeightMultiplier = 0;


        public override void AddRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate += CheckHeadAboveReward;
            agent.onFixedUpdate += AddMultiplicativeHeadHeightReward;
        }

        public override void RemoveRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate -= CheckHeadAboveReward;
            agent.onFixedUpdate -= AddMultiplicativeHeadHeightReward;
        }

        private void AddMultiplicativeHeadHeightReward(ModularAgent agent) {
            if (headHeightMultiplier != 0) {
                agent.AddReward(HeadHeight(agent) / agent.ragdoll.height * headHeightMultiplier, nameof(headHeightMultiplier), asScore);
            }
        }

        private void CheckHeadAboveReward(ModularAgent agent) {
            if (HeadHeight(agent) / agent.ragdoll.height > headAboveThreshold) {
                if (headAboveReward != 0) {
                    agent.AddReward(headAboveReward, nameof(headAboveReward), asScore);
                }
            }
        }

        float HeadHeight(ModularAgent agent) {
            return agent.ragdoll.GetLimb(LimbType.Head).position.y;
        }
    }
}
