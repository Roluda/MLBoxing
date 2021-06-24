using MLBoxing.Ragdoll;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace MLBoxing.ML.Actuators {
    public class RagdollActuatorComponent : ActuatorComponent {

        [SerializeField]
        ModularAgent observedAgent = default;
        [SerializeField, EnumFlags]
        JointType actuatedJoints = default;

        private void OnValidate() {
            if (!observedAgent) {
                observedAgent = GetComponentInParent<ModularAgent>();
            }
        }

        public override ActionSpec ActionSpec => RagdollActuator.GetActionSpec(observedAgent.ragdoll, actuatedJoints);

        [System.Obsolete]
        public override IActuator CreateActuator() {
            return new RagdollActuator() {
                actuatedRagdoll = observedAgent.ragdoll,
                actuatedJoints = actuatedJoints,
            };
        }
    }
}
