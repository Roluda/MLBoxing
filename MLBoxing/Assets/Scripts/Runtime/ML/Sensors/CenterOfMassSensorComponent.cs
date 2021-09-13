using MLBoxing.Ragdoll;
using Unity.MLAgents.Sensors;
using UnityEditor;
using UnityEngine;

namespace MLBoxing.ML.Sensors {
    public class CenterOfMassSensorComponent : SensorComponent {
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
        RagdollModel observedRagdoll = default;

        [SerializeField]
        Vector3 centerOfMassDebug = default;

        CenterOfMassSensor sensor;

        private void OnEnable() {
            observedAgent.onInitialize += Setup;
        }

        private void OnDisable() {
            observedAgent.onInitialize -= Setup;
        }

        private void Setup(ModularAgent agent) {
            switch (targetCharacter) {
                case TargetCharacter.Self:
                    SetObservedRagdoll(agent.ragdoll);
                    break;
                case TargetCharacter.Opponent:
                    SetObservedRagdoll(agent.opponent.ragdoll);
                    break;
            }
        }


        public void SetObservedRagdoll(RagdollModel ragdoll) {
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
            sensor = new CenterOfMassSensor() {
                observedRagdoll = observedRagdoll,
                name = gameObject.name,
                self = observedAgent.ragdoll,
                maxRange = observedAgent.ragdoll.height
            };
            return new StackingSensor(sensor, stackedObservations);
        }

        private void OnDrawGizmosSelected() {
            if (sensor == null) {
                return;
            }
            centerOfMassDebug = sensor.centerOfMass;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(sensor.self.root.transform.position + sensor.centerOfMass, 0.1f);
            Gizmos.DrawWireSphere(sensor.self.root.worldCenterOfMass, 0.1f);
        }

        public override int[] GetObservationShape() {
            return CenterOfMassSensor.GetShape();
        }
    }
}

