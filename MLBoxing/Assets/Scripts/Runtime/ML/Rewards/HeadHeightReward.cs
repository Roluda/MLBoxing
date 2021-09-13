using MLBoxing.Ragdoll;
using System.Linq;
using UnityEngine;

namespace MLBoxing.ML.Rewards {
    [CreateAssetMenu(fileName="R_HeadHeight_New", menuName ="ML/Rewards/HeadHeight")]
    public class HeadHeightReward : Reward {
        [SerializeField, Range(-1, 1), Tooltip("Reward each step if above threshold")]
        float aboveReward = 0.01f;
        [SerializeField, Range(0, 1), Tooltip("Threshold for height reward")]
        float aboveThreshold = 0.5f;
        [SerializeField, Range(-1, 1), Tooltip("Multipier for normalized height")]
        float heightScale = 0;
        [SerializeField, Range(-1, 1), Tooltip("Multipier for normalized of opponent height")]
        float opponentHeightScale = 0;


        public override void AddRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate += CheckHeadAboveReward;
            agent.onFixedUpdate += AddMultiplicativeHeadHeightReward;
        }

        public override void RemoveRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate -= CheckHeadAboveReward;
            agent.onFixedUpdate -= AddMultiplicativeHeadHeightReward;
        }

        private void AddMultiplicativeHeadHeightReward(ModularAgent agent) {
            if (heightScale != 0) {
                agent.AddReward(HeadHeight(agent) / agent.ragdoll.height * heightScale, nameof(heightScale), asScore);
            }
            if(opponentHeightScale != 0) {
                agent.AddReward(HeadHeight(agent.opponent) / agent.opponent.ragdoll.height * opponentHeightScale, nameof(opponentHeightScale), asScore);
            }
        }

        private void CheckHeadAboveReward(ModularAgent agent) {
            if (HeadHeight(agent) / agent.ragdoll.height > aboveThreshold) {
                if (aboveReward != 0) {
                    agent.AddReward(aboveReward, nameof(aboveReward), asScore);
                }
            }
        }

        float HeadHeight(ModularAgent agent) {
            return agent.ragdoll.GetLimb(LimbType.Head).position.y;
        }
    }
}
