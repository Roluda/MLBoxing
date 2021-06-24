using MLBoxing.Ragdoll;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML.Sensors {
    public class RagdollLimbSensor : ISensor {
        public string name;

        public RagdollModel ragdoll;
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
            return GetShape(ragdoll, observedLimbs);
        }

        public int Write(ObservationWriter writer) {
            int observations = 0;
            foreach(var limb in ragdoll.FilterLimbs(observedLimbs)) {
                var localPosition = limb.transform.position - ragdoll.root.position;
                var clampedPosition = Vector3.ClampMagnitude(localPosition, ragdoll.height);
                var normalizedPosition = clampedPosition / ragdoll.height;
                writer.Add(normalizedPosition, observations);
                observations += 3;
            }
            return observations;
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
