using MLBoxing.Character;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using UnityEngine;

namespace MLBoxing.ML {
    public class RagdollActuator : IActuator, IHeuristicProvider {

        const int actions = 16;

        public RagdollController actuatedRagdoll;

        public ActionSpec ActionSpec => GetActionSpec();

        public string Name => actuatedRagdoll.name;

        public void OnActionReceived(ActionBuffers actionBuffers) {
            int index = 0;
            actuatedRagdoll.neckX = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.neckY = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.chestX = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.chestY = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.leftShoulderX = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.leftShoulderY = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.leftElbowX = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.rightShoulderX = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.rightShoulderY = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.rightElbowX = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.leftHipX = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.leftHipY = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.leftKneeX = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.rightHipX = actionBuffers.ContinuousActions[index++];
            actuatedRagdoll.rightHipY = actionBuffers.ContinuousActions[index++];
        }

        public void ResetData() {
        }

        public void WriteDiscreteActionMask(IDiscreteActionMask actionMask) {
        }

        public static ActionSpec GetActionSpec() {
            return new ActionSpec { NumContinuousActions = actions };
        }

        public void Heuristic(in ActionBuffers actionBuffersOut) {
            throw new System.NotImplementedException();
        }
    }
}