using MLBoxing.Ragdoll;
using Unity.MLAgents.Sensors;
using UnityEditor;
using UnityEngine;

namespace MLBoxing.ML.Sensors {
    public class RagdollLimbSensorComponent : SensorComponent {
        enum TargetCharacter {
            Self,
            Opponent
        }
        [SerializeField]
        ModularAgent observedAgent = default;
        [SerializeField]
        TargetCharacter targetCharacter = TargetCharacter.Self;
        [SerializeField]
        LimbType observedLimbs = default;
        [SerializeField, Range(1, 50)]
        int stackedObservations = 1;

        [SerializeField]
        RagdollModel observedRagdoll = default;

        RagdollLimbSensor sensor;

        private void OnEnable() {
            observedAgent.onInitialize += Setup;
        }

        private void OnDisable() {
            observedAgent.onInitialize -= Setup;
        }

        private void Setup(ModularAgent agent) {
            switch (targetCharacter) {
                case TargetCharacter.Self:
                    SetObservedCharacter(agent.ragdoll);
                    break;
                case TargetCharacter.Opponent:
                    SetObservedCharacter(agent.opponent.ragdoll);
                    break;
            }
        }

        public void SetObservedCharacter(RagdollModel ragdoll) {
            observedRagdoll = ragdoll;
            if (sensor != null) {
                sensor.observedRagdoll = ragdoll;
            }
        }

        void OnValidate() {
            if (!observedAgent) {
                observedAgent = GetComponentInParent<ModularAgent>();
            }
        }

        public override ISensor CreateSensor() {
            sensor = new RagdollLimbSensor() {
                observedRagdoll = observedRagdoll,
                name = gameObject.name,
                self = observedAgent.ragdoll,
                maxRange = observedAgent.ragdoll.height,
                observedLimbs = observedLimbs
            };
            return new StackingSensor(sensor, stackedObservations);
        }

        private void OnDrawGizmosSelected() {
            if (sensor == null) {
                return;
            }
            foreach (var position in sensor.NormalizedPositionsSubjective()) {
                Gizmos.color = Color.Lerp(Color.white, Color.red, position.magnitude);
                Gizmos.DrawLine(sensor.self.rootPosition, sensor.self.rootPosition + position);
            }
        }

        public override int[] GetObservationShape() {
            return RagdollLimbSensor.GetShape(observedRagdoll, observedLimbs);
        }
    }
}
