using MLBoxing.Ragdoll;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML.Sensors {
    public class RagdollJointSensor : ISensor {
        public RagdollModel ragdoll;
        public string name;
        public JointType observedJoints = default;

        public byte[] GetCompressedObservation() {
            Debug.LogError("This Sensor does not implement a compressed Observation");
            return null;
        }

        public SensorCompressionType GetCompressionType() {
            return SensorCompressionType.None;
        }

        public string GetName() {
            return name;
        }

        public int[] GetObservationShape() {
            return GetShape(ragdoll, observedJoints);
        }

        public int Write(ObservationWriter writer) {
            List<float> observations = GetAllCurrentJointRotationsNormalized().ToList();
            writer.AddList(observations);
            return observations.Count;
        }

        //Inherited
        public void Reset() {}

        //Inherited
        void ISensor.Update() {}

        public IEnumerable<float> GetAllCurrentJointRotationsNormalized() {
            foreach(var joint in ragdoll.FilterJoints(observedJoints)) {
                if (joint.dof.HasFlag(DOF.X)) {
                    yield return joint.GetNormalizedJointPosition(DOF.X);
                }
                if (joint.dof.HasFlag(DOF.Y)) {
                    yield return joint.GetNormalizedJointPosition(DOF.Y);
                }
                if (joint.dof.HasFlag(DOF.Z)) {
                    yield return joint.GetNormalizedJointPosition(DOF.Z);
                }
            }
        }

        public static int[] GetShape(RagdollModel ragdoll, JointType joints) {
            return new int[] { ragdoll.FilterJoints(joints).Sum(joint => joint.dofCount)};
        }
    }
}
