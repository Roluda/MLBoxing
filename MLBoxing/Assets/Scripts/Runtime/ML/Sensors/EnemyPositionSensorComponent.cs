using MLBoxing.Ragdoll;
using Unity.MLAgents.Sensors;
using UnityEditor;
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
        [SerializeField]
        float maxRange = 5;

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
                name = gameObject.name,
                maxRange = maxRange
            };
            return new StackingSensor(sensor, stackedObservations);
        }

        private void OnDrawGizmosSelected() {
            if (sensor == null) {
                return;
            }
            float normalizedValue = sensor.enemyPosition.magnitude;
            Gizmos.color = Color.Lerp(Color.white, Color.green, normalizedValue);
            Gizmos.DrawLine(sensor.self.rootPosition, sensor.self.rootPosition + sensor.enemyPosition);
        }

        public override int[] GetObservationShape() {
            return EnemyPositionSensor.GetShape();
        }
    }
}
