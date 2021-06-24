using MLBoxing.Character;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML {
    public class CenterOfMassSensor : ISensor {
        public string name;

        public RagdollController ragdoll;
        public BoxingCharacter character;

        public Vector3 centerOfMass {
            get {
                return ragdoll
                    ? ragdoll.allLimbs.Aggregate(Vector3.zero, (s, v) => s + v.mass * v.position) / ragdoll.allLimbs.Sum(rigid => rigid.mass)
                    : Vector3.zero;
            }
        }

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
            var localPosition = centerOfMass - character.position;
            var clampedPosition = Vector3.ClampMagnitude(localPosition, character.height);
            var normalizedPosition = clampedPosition / character.height;
            writer.Add(normalizedPosition);
            return 3;
        }

        Vector3 CalculateCenterOfMass(RagdollController ragdoll) {
            return ragdoll.allLimbs.Aggregate(Vector3.zero, (s, v) => s + v.mass * v.position) / ragdoll.allLimbs.Sum(rigid => rigid.mass);
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