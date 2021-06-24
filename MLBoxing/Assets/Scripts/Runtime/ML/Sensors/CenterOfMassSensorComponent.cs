using MLBoxing.Ragdoll;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
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
                sensor.ragdoll = ragdoll;
            }
        }

        void OnValidate() {
            if (!observedAgent) {
                observedAgent = GetComponentInParent<ModularAgent>();
            }
        }

        public override ISensor CreateSensor() {
            sensor = new CenterOfMassSensor() {
                ragdoll = observedRagdoll,
                name = gameObject.name
            };
            return new StackingSensor(sensor, stackedObservations);
        }

        private void OnDrawGizmosSelected() {
            if (sensor == null) {
                return;
            }
            centerOfMassDebug = sensor.centerOfMass;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(sensor.centerOfMass, 0.3f);
        }

        public override int[] GetObservationShape() {
            return CenterOfMassSensor.GetShape();
        }
    }
}

