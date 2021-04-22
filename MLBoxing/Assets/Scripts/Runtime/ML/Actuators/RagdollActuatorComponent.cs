using MLBoxing.Character;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace MLBoxing.ML {
    public class RagdollActuatorComponent : ActuatorComponent {

        [SerializeField]
        ModularAgent observedAgent = default;

        private void OnValidate() {
            if (!observedAgent) {
                observedAgent = GetComponentInParent<ModularAgent>();
            }
        }

        public override ActionSpec ActionSpec => RagdollActuator.GetActionSpec();

        [System.Obsolete]
        public override IActuator CreateActuator() {
            return new RagdollActuator() {
                actuatedRagdoll = observedAgent.controller
            };
        }
    }
}
