using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML.Rewards {
    [CreateAssetMenu(fileName = "R_OpponentDistance_Short", menuName = "ML/Rewards/OpponentDistance")]
    public class OpponentDistanceReward : Reward {

        [SerializeField, Tooltip("Closer = Higher Base")]
        float oppoenentDistanceMultiplier = 0.01f;
        [SerializeField]
        float maximumDistance = 5f;

        public override void AddRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate += AddMultiplicativeOpponentDistanceReward;
        }

        public override void RemoveRewardListeners(ModularAgent agent) {
            agent.onFixedUpdate -= AddMultiplicativeOpponentDistanceReward;
        }

        private void AddMultiplicativeOpponentDistanceReward(ModularAgent agent) {
            if (oppoenentDistanceMultiplier != 0) {
                float distance = (agent.opponent.ragdoll.root.position - agent.ragdoll.root.position).magnitude;
                float clampedDistance = Mathf.Clamp(distance, 0, maximumDistance);
                float normalizedDistance = clampedDistance / maximumDistance;
                agent.AddReward((1 - normalizedDistance) * oppoenentDistanceMultiplier, nameof(oppoenentDistanceMultiplier), asScore);
            }
        }
    }
}
