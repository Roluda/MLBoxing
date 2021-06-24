using UnityEngine;

namespace MLBoxing.ML.Rewards {
    [CreateAssetMenu(fileName = "R_Existence_New", menuName = "ML/Rewards/Existence")]
    public class Existance : Reward {
        [SerializeField, Range(-1, 1)]
        float existantialReward = 0;

        public override void AddRewardListeners(ModularAgent agent) {
            if (existantialReward != 0) {
                agent.onFixedUpdate += AddExistentialReward;
            }
        }

        public override void RemoveRewardListeners(ModularAgent agent) {
            if (existantialReward != 0) {
                agent.onFixedUpdate -= AddExistentialReward;
            }
        }

        private void AddExistentialReward(ModularAgent agent) {
            if (existantialReward != 0) {
                agent.AddReward(existantialReward, nameof(existantialReward), asScore);
            }
        }
    }
}
