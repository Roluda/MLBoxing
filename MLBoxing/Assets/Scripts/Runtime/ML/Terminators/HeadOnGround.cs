using MLBoxing.Ragdoll;
using MLBoxing.ML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MLBoxing.ML.Terminators {
    [CreateAssetMenu(fileName = "T_HeadOnGround_New", menuName = "ML/Terminaters/HeadOnGround")]
    public class HeadOnGround : Terminator {
        [SerializeField]
        float groundHeight = 0.3f;

        public override void AddTerminationListeners(ModularAgent agent) {
            agent.onFixedUpdate += CheckHeadOnGround;
        }

        public override void RemoveTerminationListeners(ModularAgent agent) {
            agent.onFixedUpdate -= CheckHeadOnGround;
        }

        private void CheckHeadOnGround(ModularAgent agent) {
            if (HeadHeight(agent)/agent.ragdoll.height < groundHeight) {
                agent.Terminate();
            }
        }

        float HeadHeight(ModularAgent agent) {
            return agent.ragdoll.FilterLimbs(LimbType.Head).First().position.y;
        }
    }
}
