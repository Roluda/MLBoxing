using MLBoxing.Ragdoll;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace MLBoxing.ML.Sensors {
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
        JointType observedJoints = default;
        [SerializeField]
        RagdollModel observedRagdoll = default;
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
                    SetObservedRagdoll(agent.ragdoll);
                    break;
                case TargetRagdoll.Opponent:
                    SetObservedRagdoll(agent.opponent.ragdoll);
                    break;
            }
        }

        public void SetObservedRagdoll(RagdollModel ragdoll) {
            observedRagdoll = ragdoll;
            if (sensor!= null) {
                sensor.ragdoll = ragdoll;
            }
        }

        public override ISensor CreateSensor() {
            sensor = new RagdollJointSensor() {
                ragdoll = observedRagdoll,
                name = gameObject.name,
                observedJoints = observedJoints
            };
            return new StackingSensor(sensor, stackedObservations);
        }

        public override int[] GetObservationShape() {
            return RagdollJointSensor.GetShape(observedRagdoll, observedJoints);
        }
    }
}
