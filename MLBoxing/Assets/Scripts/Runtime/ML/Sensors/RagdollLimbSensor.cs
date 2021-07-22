using MLBoxing.Ragdoll;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML.Sensors {
    public class RagdollLimbSensor : ISensor {
        public string name;

        public float maxRange;
        public RagdollModel self;
        public RagdollModel observedRagdoll;
        public LimbType observedLimbs;

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
            return GetShape(observedRagdoll, observedLimbs);
        }

        public int Write(ObservationWriter writer) {
            int observations = 0;
            foreach(var position in NormalizedPositionsSubjective()) {
                writer.Add(position);
                observations += 3;
            }
            return observations;
        }

        public IEnumerable<Vector3> NormalizedPositionsSubjective() {
            foreach(var limb in observedRagdoll.FilterLimbs(observedLimbs)) {
                yield return SensorUtility.GetNormalizedSubjectivePosition(self, limb.position, maxRange);
            }
        }

        //Inherited
        public void Reset() { }

        //Inherited
        void ISensor.Update() { }

        public static int[] GetShape(RagdollModel ragdoll, LimbType types) {
            return new int[] { ragdoll.FilterLimbs(types).Count() * 3 };
        }
    }
}
