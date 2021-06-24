using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML.Rewards {
    [CreateAssetMenu(fileName = "R_Combat_New", menuName = "ML/Rewards/Combat")]
    public class CombatReward : Reward {
        [SerializeField]
        float dealDamageMultiplier = 1;
        [SerializeField]
        float takeDamageMultiplier = -1;

        public override void AddRewardListeners(ModularAgent agent) {
            agent.onDealDamage += DealDamageReward;
            agent.onTakeDamage += TakeDamageReward;
        }
        public override void RemoveRewardListeners(ModularAgent agent) {
            agent.onDealDamage -= DealDamageReward;
            agent.onTakeDamage -= TakeDamageReward;
        }

        void DealDamageReward(ModularAgent agent, float damage) {
            if (dealDamageMultiplier != 0) {
                agent.AddReward(damage * dealDamageMultiplier, nameof(dealDamageMultiplier), asScore);
            }
        }

        void TakeDamageReward(ModularAgent agent, float damage) {
            if (takeDamageMultiplier != 0) {
                agent.AddReward(damage * takeDamageMultiplier, nameof(takeDamageMultiplier), asScore);
            }
        }
    }
}
