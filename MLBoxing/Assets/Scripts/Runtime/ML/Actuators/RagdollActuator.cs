using MLBoxing.Ragdoll;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Actuators;

namespace MLBoxing.ML.Actuators {
    public class RagdollActuator : IActuator {
        public JointType actuatedJoints = default;

        public RagdollModel actuatedRagdoll;

        public ActionSpec ActionSpec => GetActionSpec(actuatedRagdoll, actuatedJoints);

        public string Name => actuatedRagdoll.name;

        public void OnActionReceived(ActionBuffers actionBuffers) {
            int index = 0;
            foreach(var joint in actuatedRagdoll.FilterJoints(actuatedJoints)) {
                if (joint.dof.HasFlag(DOF.X)) {
                    joint.SetInputX(actionBuffers.ContinuousActions[index++]);
                }
                if (joint.dof.HasFlag(DOF.Y)) {
                    joint.SetInputY(actionBuffers.ContinuousActions[index++]);
                }
                if (joint.dof.HasFlag(DOF.Z)) {
                    joint.SetInputZ(actionBuffers.ContinuousActions[index++]);
                }
            }
        }

        public void ResetData() {
        }

        public void WriteDiscreteActionMask(IDiscreteActionMask actionMask) {
        }

        public static ActionSpec GetActionSpec(RagdollModel ragdoll, JointType joints) {
            return new ActionSpec { NumContinuousActions = ragdoll.FilterJoints(joints).Sum(joint => joint.dofCount)};
        }
    }
}
