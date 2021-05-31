using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML {
    [CreateAssetMenu(fileName = "S_HeadHeight_New", menuName = "ML/Scores/HeadHeight")]
    public class HeadHeightScore : Score {
        [SerializeField, Range(-1, 1), Tooltip("Reward each step if above threshold")]
        float headAboveScore = 0.01f;
        [SerializeField, Range(0, 1), Tooltip("Threshold for height reward")]
        float headAboveThreshold = 0.5f;
        [SerializeField, Range(-1, 1), Tooltip("Multipier for normalized height")]
        float headHeightMultiplier = 0;


        public override void AddScoreListeners(ModularAgent agent) {
            agent.onFixedUpdate += CheckHeadAboveReward;
            agent.onFixedUpdate += AddMultiplicativeHeadHeightReward;
        }

        private void AddMultiplicativeHeadHeightReward(ModularAgent agent) {
            if (headHeightMultiplier != 0) {
                agent.AddScore(agent.character.head.transform.position.y / agent.character.height * headHeightMultiplier);
            }
        }

        private void CheckHeadAboveReward(ModularAgent agent) {
            if (agent.character.head.transform.position.y / agent.character.height > headAboveThreshold) {
                agent.AddScore(headAboveScore);
            }
        }
    }
}
