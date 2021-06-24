using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML {
    [CreateAssetMenu(fileName = "L_new", menuName="ML/Lesson")]
    public class Lesson : ScriptableObject {
        [SerializeField]
        public ModularAgent student = default;
        [SerializeField]
        public List<Reward> rewards = default;
        [SerializeField]
        public List<Terminater> terminaters = default;
        [SerializeField]
        public List<Score> scoreSources = default;
        [SerializeField]
        public int episodeLength = 600;
        [SerializeField]
        public bool selfPlay = false;
        [SerializeField]
        public bool mirrorOpponent = false;
    }
}
