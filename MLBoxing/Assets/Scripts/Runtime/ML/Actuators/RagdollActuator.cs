using MLBoxing.Character;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Actuators;

namespace MLBoxing.ML {
    public class RagdollActuator : IActuator {
        public JointType actuatedJoints = default;

        public RagdollController actuatedRagdoll;

        public ActionSpec ActionSpec => GetActionSpec(actuatedRagdoll, actuatedJoints);

        public string Name => actuatedRagdoll.name;

        public void OnActionReceived(ActionBuffers actionBuffers) {
            int index = 0;
            foreach(var joint in actuatedRagdoll.FilterJoints(actuatedJoints)) {
                joint.positionInputX = actionBuffers.ContinuousActions[index++];
                joint.positionInputY = actionBuffers.ContinuousActions[index++];
            }
        }

        public void ResetData() {
        }

        public void WriteDiscreteActionMask(IDiscreteActionMask actionMask) {
        }

        public static ActionSpec GetActionSpec(RagdollController ragdoll, JointType joints) {
            return new ActionSpec { NumContinuousActions = ragdoll.FilterJoints(joints).Count() * 2};
        }
    }
}
