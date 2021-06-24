using MLBoxing.Character;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML {
    public class RagdollJointSensor : ISensor {
        public RagdollController ragdoll;
        public string name;
        public JointType observedJoints;

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
                yield return joint.GetNormalizedJointRotationX();
                yield return joint.GetNormalizedJointRotationY();
            }
        }

        public static int[] GetShape(RagdollController ragdoll, JointType joints) {
            return new int[] { ragdoll.FilterJoints(joints).Count() *2 };
        }
    }
}
