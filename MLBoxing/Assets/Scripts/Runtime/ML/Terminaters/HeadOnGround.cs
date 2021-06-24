using MLBoxing.ML;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML {
    [CreateAssetMenu(fileName = "T_HeadOnGround_New", menuName = "ML/Terminaters/HeadOnGround")]
    public class HeadOnGround : Terminater {
        [SerializeField]
        float groundHeight = 0.3f;

        public override void AddTerminationListeners(ModularAgent agent) {
            agent.onFixedUpdate += CheckHeadOnGround;
        }

        public override void RemoveTerminationListeners(ModularAgent agent) {
            agent.onFixedUpdate -= CheckHeadOnGround;
        }

        private void CheckHeadOnGround(ModularAgent agent) {
            if (agent.character.head.transform.position.y < groundHeight) {
                agent.Terminate();
            }
        }
    }
}
