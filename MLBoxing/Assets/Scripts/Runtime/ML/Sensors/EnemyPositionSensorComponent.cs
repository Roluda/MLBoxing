using MLBoxing.Character;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML {
    public class EnemyPositionSensorComponent : SensorComponent {

        [SerializeField]
        ModularAgent observedAgent = default;
        [SerializeField, Range(1, 50)]
        int stackedObservations = 1;

        [SerializeField]
        BoxingCharacter observedCharacter = default;
        [SerializeField]
        BoxingCharacter observedEnemyCharacter = default;

        EnemyPositionSensor sensor;

        private void OnEnable() {
            observedAgent.onInitialize += Setup;
        }

        private void OnDisable() {
            observedAgent.onInitialize -= Setup;
        }

        private void Setup(ModularAgent agent) {
            SetObservedCharacters(agent.character, agent.opponent.character);
        }

        public void SetObservedCharacters(BoxingCharacter character, BoxingCharacter enemy) {
            observedCharacter = character;
            observedEnemyCharacter = enemy;
            if (sensor != null) {
                sensor.character = character;
                sensor.enemyCharacter = enemy;
            }
        }

        void OnValidate() {
            if (!observedAgent) {
                observedAgent = GetComponentInParent<ModularAgent>();
            }
        }

        public override ISensor CreateSensor() {
            sensor = new EnemyPositionSensor() {
                character = observedCharacter,
                enemyCharacter = observedEnemyCharacter,
                name = gameObject.name
            };
            return new StackingSensor(sensor, stackedObservations);
        }

        public override int[] GetObservationShape() {
            return EnemyPositionSensor.GetShape();
        }
    }
}
