using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML {
    [CreateAssetMenu(fileName = "R_ApproachOpponent_New", menuName = "ML/Rewards/ApproachOpponent")]
    public class ApproachOpponentReward : Reward {
        [SerializeField]
        float appreachEnemyReward = 0;
        [SerializeField]
        float enemyApproachesReward = 0;

        public override void AddRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate += AddApproachRewards;
        }

        public override void RemoveRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate -= AddApproachRewards;
        }

        void AddApproachRewards(ModularAgent agent) {
            if (appreachEnemyReward != 0) {
                var direction = agent.opponent.ragdoll.rootPosition - agent.ragdoll.rootPosition;
                float angle = Vector3.Angle(direction, agent.ragdoll.deltaPosition);
                float magnitude = agent.ragdoll.deltaPosition.magnitude;
                float reward = (180 - angle) * magnitude * appreachEnemyReward;
                agent.AddReward(reward, nameof(appreachEnemyReward), asScore);
            }
            if (enemyApproachesReward != 0) {
                var direction = agent.ragdoll.rootPosition - agent.opponent.ragdoll.rootPosition;
                float angle = Vector3.Angle(direction, agent.opponent.ragdoll.deltaPosition);
                float magnitude = agent.ragdoll.deltaPosition.magnitude;
                float reward = (180 - angle) * magnitude * enemyApproachesReward;
                agent.AddReward(reward, nameof(enemyApproachesReward), asScore);
            }
        }
    }
}
