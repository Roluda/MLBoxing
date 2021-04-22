using MLBoxing.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML {
    public class RagdollJointSensorComponent : SensorComponent {
        enum TargetRagdoll {
            Self,
            Opponent
        }

        [SerializeField]
        ModularAgent observedAgent;
        [SerializeField, Range(1, 50)]
        int stackedObservations = 1;
        [SerializeField]
        TargetRagdoll targetRagdoll = TargetRagdoll.Self;

        [SerializeField]
        RagdollController observedRagdoll = default;
        RagdollJointSensor sensor;

        private void OnValidate() {
            if (!observedAgent) {
                observedAgent = GetComponentInParent<ModularAgent>();
            }
        }

        private void OnEnable() {
            observedAgent.onInitialize += Setup;
        }

        private void OnDisable() {
            observedAgent.onInitialize -= Setup;
        }

        private void Setup(ModularAgent agent) {
            switch (targetRagdoll) {
                case TargetRagdoll.Self:
                    SetObservedRagdoll(agent.controller);
                    break;
                case TargetRagdoll.Opponent:
                    SetObservedRagdoll(agent.opponent.controller);
                    break;
            }
        }

        public void SetObservedRagdoll(RagdollController ragdoll) {
            observedRagdoll = ragdoll;
            if (sensor!= null) {
                sensor.ragdoll = ragdoll;
            }
        }

        public override ISensor CreateSensor() {
            sensor = new RagdollJointSensor() {
                ragdoll = observedRagdoll,
                name = gameObject.name
            };
            return new StackingSensor(sensor, stackedObservations);
        }

        public override int[] GetObservationShape() {
            return RagdollJointSensor.GetShape(observedRagdoll);
        }
    }
}
