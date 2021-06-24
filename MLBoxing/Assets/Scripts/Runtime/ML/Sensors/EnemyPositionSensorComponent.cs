using MLBoxing.Ragdoll;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML.Sensors {
    public class EnemyPositionSensorComponent : SensorComponent {

        [SerializeField]
        ModularAgent observedAgent = default;
        [SerializeField, Range(1, 50)]
        int stackedObservations = 1;

        [SerializeField]
        RagdollModel self = default;
        [SerializeField]
        RagdollModel enemy = default;

        EnemyPositionSensor sensor;

        private void OnEnable() {
            observedAgent.onInitialize += Setup;
        }

        private void OnDisable() {
            observedAgent.onInitialize -= Setup;
        }

        private void Setup(ModularAgent agent) {
            SetObservedRagdolls(agent.ragdoll, agent.opponent.ragdoll);
        }

        public void SetObservedRagdolls(RagdollModel self, RagdollModel enemy) {
            this.self = self;
            this.enemy = enemy;
            if (sensor != null) {
                sensor.self = self;
                sensor.enemy = enemy;
            }
        }

        void OnValidate() {
            if (!observedAgent) {
                observedAgent = GetComponentInParent<ModularAgent>();
            }
        }

        public override ISensor CreateSensor() {
            sensor = new EnemyPositionSensor() {
                self = self,
                enemy = enemy,
                name = gameObject.name
            };
            return new StackingSensor(sensor, stackedObservations);
        }

        public override int[] GetObservationShape() {
            return EnemyPositionSensor.GetShape();
        }
    }
}
