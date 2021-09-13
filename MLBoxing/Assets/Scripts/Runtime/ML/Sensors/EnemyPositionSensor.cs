using MLBoxing.Ragdoll;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML.Sensors {
    public class EnemyPositionSensor : ISensor {
        public string name;

        public RagdollModel self;
        public RagdollModel enemy;

        public float maxRange = 5;

        public Vector3 enemyPosition => SensorUtility.GetNormalizedSubjectivePosition(self, enemy.rootPosition, maxRange);

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
            if (enemy) {
                writer.Add(enemyPosition);
            } else {
                writer.Add(Vector3.zero);
            }
            return 3;
        }


        //Inherited
        public void Reset() { }

        //Inherited
        void ISensor.Update() { }

        public static int[] GetShape() {
            return new int[] {3};
        }
    }
}
