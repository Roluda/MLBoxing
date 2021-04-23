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
            return GetShape(ragdoll);
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
            foreach(var joint in ragdoll.allJoints) {
                yield return ragdoll.GetNormalizedJointRotationX(joint);
                if(joint.angularYLimit.limit > 0) {
                    yield return ragdoll.GetNormalizedJointRotationY(joint);
                }
            }
        }

        public static int[] GetShape(RagdollController ragdoll) {
            int movingAxisCount = 0;
            foreach (var joint in ragdoll.allJoints) {
                movingAxisCount++;
                if (joint.angularYLimit.limit > 0) {
                    movingAxisCount++;
                }
            }
            return new int[] { movingAxisCount };
        }
    }
}
