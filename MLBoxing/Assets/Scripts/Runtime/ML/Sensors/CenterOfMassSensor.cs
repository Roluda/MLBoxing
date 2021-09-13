using MLBoxing.Ragdoll;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML.Sensors {
    public class CenterOfMassSensor : ISensor {
        public string name;

        public float maxRange;
        public RagdollModel self;
        public RagdollModel observedRagdoll;

        public Vector3 centerOfMass => SensorUtility.GetNormalizedSubjectivePosition(self, CalculateCenterOfMass(observedRagdoll), maxRange);

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
            return GetShape();
        }

        public int Write(ObservationWriter writer) {
            writer.Add(centerOfMass);
            return 3;
        }

        Vector3 CalculateCenterOfMass(RagdollModel ragdoll) {
            if (!ragdoll) {
                return Vector3.zero;
            }
            return ragdoll.allArticulations.Aggregate(Vector3.zero, (s, v) => s + v.mass * v.position) / ragdoll.allArticulations.Sum(rigid => rigid.mass);
            //return ragdoll.root.worldCenterOfMass;
        }

        //Inherited
        public void Reset() { }

        //Inherited
        void ISensor.Update() { }

        public static int[] GetShape() {
            return new int[] { 3 };
        }
    }
}