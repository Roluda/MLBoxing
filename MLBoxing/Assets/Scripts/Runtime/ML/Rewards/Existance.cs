using UnityEngine;

namespace MLBoxing.ML {
    [CreateAssetMenu(fileName = "R_Existence_New", menuName = "ML/Rewards/Existence")]
    public class Existance : Reward {
        [SerializeField, Range(-1, 1)]
        float existentialReward = 0;

        public override void AddRewardListeners(ModularAgent agent) {
            if (existentialReward != 0) {
                agent.onEpisodeStep += AddExistentialReward;
            }
        }

        private void AddExistentialReward(ModularAgent agent) {
            agent.AddReward(existentialReward);
        }
    }
}
