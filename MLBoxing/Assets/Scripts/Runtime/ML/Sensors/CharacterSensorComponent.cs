using MLBoxing.Character;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML {
    public class CharacterSensorComponent : SensorComponent {
        enum TargetCharacter {
            Self,
            Opponent
        }
        [SerializeField]
        ModularAgent observedAgent = default;
        [SerializeField]
        TargetCharacter targetCharacter = TargetCharacter.Self;
        [SerializeField, Range(1, 50)]
        int stackedObservations = 1;

        [SerializeField]
        BoxingCharacter observedCharacter = default;

        CharacterSensor sensor;

        private void OnEnable() {
            observedAgent.onInitialize += Setup;
        }

        private void OnDisable() {
            observedAgent.onInitialize -= Setup;
        }

        private void Setup(ModularAgent agent) {
            switch (targetCharacter) {
                case TargetCharacter.Self:
                    SetObservedCharacter(agent.character);
                    break;
                case TargetCharacter.Opponent:
                    SetObservedCharacter(agent.opponent.character);
                    break;
            }
        }

        public void SetObservedCharacter(BoxingCharacter character) {
            observedCharacter = character;
            if (sensor != null) {
                sensor.character = character;
            }
        }

        void OnValidate() {
            if (!observedAgent) {
                observedAgent = GetComponentInParent<ModularAgent>();
            }
        }

        public override ISensor CreateSensor() {
            sensor = new CharacterSensor() {
                character = observedCharacter,
                name = gameObject.name
            };
            return new StackingSensor(sensor, stackedObservations);
        }

        public override int[] GetObservationShape() {
            return CharacterSensor.GetShape(observedCharacter);
        }
    }
}
