using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML.Rewards {
    [CreateAssetMenu(fileName = "R_Competition_New", menuName = "ML/Rewards/Competition")]
    public class CompetitionReward : Reward {

        [SerializeField]
        float winReward = 1;
        [SerializeField]
        float loseReward = -1;

        public override void AddRewardListeners(ModularAgent agent) {
            agent.onWin += AddWinReward;
            agent.onLose += AddLoseReward;
        }

        public override void RemoveRewardListeners(ModularAgent agent) {
            agent.onWin -= AddWinReward;
            agent.onLose -= AddLoseReward;
        }

        void AddWinReward(ModularAgent agent) {
            if (winReward != 0) {
                agent.AddReward(winReward, nameof(winReward));
            }
        }

        void AddLoseReward(ModularAgent agent) {
            if (loseReward != 0) {
                agent.AddReward(loseReward, nameof(loseReward));
            }
        }


    }
}
