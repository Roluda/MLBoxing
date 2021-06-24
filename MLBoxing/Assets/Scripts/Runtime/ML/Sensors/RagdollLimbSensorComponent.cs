using MLBoxing.Ragdoll;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
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
                sensor.ragdoll = ragdoll;
            }
        }

        void OnValidate() {
            if (!observedAgent) {
                observedAgent = GetComponentInParent<ModularAgent>();
            }
        }

        public override ISensor CreateSensor() {
            sensor = new RagdollLimbSensor() {
                ragdoll = observedRagdoll,
                name = gameObject.name,
                observedLimbs = observedLimbs
            };
            return new StackingSensor(sensor, stackedObservations);
        }

        public override int[] GetObservationShape() {
            return RagdollLimbSensor.GetShape(observedRagdoll, observedLimbs);
        }
    }
}
