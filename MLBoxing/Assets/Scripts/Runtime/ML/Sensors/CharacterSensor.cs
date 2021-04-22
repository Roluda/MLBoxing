using MLBoxing.Character;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML {
    public class CharacterSensor : ISensor {
        public string name;

        public BoxingCharacter character;

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
            return GetShape(character);
        }

        public int Write(ObservationWriter writer) {
            int observations = 0;
            foreach(var limb in character.allLimbs) {
                observations += 3;
                var localPosition = limb.transform.position - character.position;
                var clampedPosition = Vector3.ClampMagnitude(localPosition, character.height);
                var normalizedPosition = clampedPosition / character.height;
                writer.Add(normalizedPosition);
            }
            return observations;
        }

        //Inherited
        public void Reset() { }

        //Inherited
        void ISensor.Update() { }

        public static int[] GetShape(BoxingCharacter character) {
            return new int[character.allLimbs.ToList().Count * 3];
        }
    }
}
