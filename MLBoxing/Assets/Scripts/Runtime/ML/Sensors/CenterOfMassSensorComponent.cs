using MLBoxing.Character;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML {
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
        RagdollController observedRagdoll = default;
        [SerializeField]
        BoxingCharacter observedCharacter = default;

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
                    SetObservedRagdoll(agent.controller);
                    SetObservedCharacter(agent.character);
                    break;
                case TargetCharacter.Opponent:
                    SetObservedRagdoll(agent.opponent.controller);
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

        public void SetObservedRagdoll(RagdollController ragdoll) {
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
                character = observedCharacter,
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

