using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MLBoxing.Ragdoll {
    public class RagdollJoint : MonoBehaviour {
        [SerializeField]
        public ConfigurableJoint joint = default;
        [SerializeField]
        public JointType jointType = default;
        [SerializeField]
        public DOF axis = default;
        [SerializeField, Range(0, 1)]
        public float inputX = 0;
        [SerializeField, Range(0, 1)]
        public float inputY = 0;
        [SerializeField, Range(0, 1)]
        public float inputZ = 0;
        [SerializeField]
        ControlType control = ControlType.PositionControl;
        [SerializeField]
        float maxAngularVelocity = 50;
        [SerializeField]
        float maxAngularForce = 1000;
        [SerializeField]
        int velocityMeasurements = 100;

        Quaternion lastRotation;

        Quaternion startRotation;

        [SerializeField]
        public Vector3 jointRotation;


        public int axisCount = 0;

        Queue<Vector3> velocityMeasures = new Queue<Vector3>();

        public float currentTorque => joint.currentTorque.sqrMagnitude;



        private void Start() {
            EnterControlMode(control);
        }

        /*
        target Position = ((force + PositionDamper * Velocity) / PositionSpring) + position
        */

        private void OnValidate() {
            var right = joint.axis;
            var forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
            var up = Vector3.Cross(forward, right).normalized;
            jointRotation = Quaternion.LookRotation(joint.axis, joint.secondaryAxis).eulerAngles;
            if (!joint) {
                joint = GetComponent<ConfigurableJoint>();
            }
            axis = 0;
            if(joint.lowAngularXLimit.limit != 0 || joint.highAngularXLimit.limit != 0) {
                axis |= DOF.X;
            }
            if(joint.angularYLimit.limit > 0) {
                axis |= DOF.Y;
            }
            if(joint.angularZLimit.limit > 0) {
                axis |= DOF.Z;
            }
            axisCount = GetSetBitCount((int)axis);
        }
        private void Awake() {
            startRotation = transform.rotation;
        }

        private void FixedUpdate() {
            switch (control) {
                case ControlType.PositionControl:
                    SetTargetRotation();
                    break;
                case ControlType.VelocityControl:
                    SetTargetVelocity();
                    break;
                case ControlType.AccelerationControl:
                    AccelerateJoint();
                    break;
                default:
                    throw new NotImplementedException($"control of type {control} is not implemented");
            }
        }
        public void EnterControlMode(ControlType controlType) {
            control = controlType;
            var slerpDrive = joint.slerpDrive;
            switch (controlType) {
                case ControlType.PositionControl:
                    inputX = GetNormalizedJointRotationX();
                    inputY = GetNormalizedJointRotationY();
                    inputZ = GetNormalizedJointRotationZ();
                    slerpDrive.positionSpring = maxAngularForce;
                    slerpDrive.positionDamper = 0;
                    joint.slerpDrive = slerpDrive;
                    break;
                case ControlType.VelocityControl:
                    inputX = 0.5f;
                    inputY = 0.5f;
                    inputZ = 0.5f;
                    slerpDrive.positionSpring = 0;
                    slerpDrive.positionDamper = maxAngularVelocity;
                    joint.slerpDrive = slerpDrive;
                    break;
                case ControlType.AccelerationControl:
                    inputX = 0.5f;
                    inputY = 0.5f;
                    inputZ = 0.5f;
                    lastRotation = joint.GetJointRotation(startRotation);
                    for (int i = 0; i < velocityMeasurements; i++) {
                        velocityMeasures.Enqueue(Vector3.zero);
                    }
                    slerpDrive.positionSpring = 0;
                    slerpDrive.positionDamper = maxAngularVelocity;
                    joint.slerpDrive = slerpDrive;
                    break;
            }
        }
        public float GetNormalizedJointRotationX() {
            float min = joint.lowAngularXLimit.limit;
            float max = joint.highAngularXLimit.limit;
            float current = joint.GetJointRotation(startRotation).eulerAngles.x;
            return Mathf.InverseLerp(min, max, current);
        }

        public float GetNormalizedJointRotationY() {
            float min = joint.angularYLimit.limit;
            float max = -joint.angularYLimit.limit;
            float current = joint.GetJointRotation(startRotation).eulerAngles.y;
            return Mathf.InverseLerp(min, max, current);
        }

        public float GetNormalizedJointRotationZ() {
            float min = joint.angularZLimit.limit;
            float max = -joint.angularZLimit.limit;
            float current = joint.GetJointRotation(startRotation).eulerAngles.z;
            return Mathf.InverseLerp(min, max, current);
        }
        void SetTargetRotation() {
            float positionX = Mathf.Lerp(joint.lowAngularXLimit.limit, joint.highAngularXLimit.limit, inputX);
            float positionY = Mathf.Lerp(-joint.angularYLimit.limit, joint.angularYLimit.limit, inputY);
            float positionZ = Mathf.Lerp(-joint.angularZLimit.limit, joint.angularZLimit.limit, inputZ);
            joint.targetRotation = Quaternion.Euler(positionX, positionY, positionZ);
        }

        void SetTargetVelocity() {
            float velocityX = Mathf.Lerp(-maxAngularVelocity, maxAngularVelocity, inputX);
            float velocityY = Mathf.Lerp(-maxAngularVelocity, maxAngularVelocity, inputY);
            float velocityZ = Mathf.Lerp(-maxAngularVelocity, maxAngularVelocity, inputZ);
            joint.targetAngularVelocity = new Vector3(velocityX, velocityY, velocityZ);
        }

        void AccelerateJoint() {
            var deltaRotation = joint.GetJointRotation(startRotation) * Quaternion.Inverse(lastRotation);
            velocityMeasures.Enqueue(deltaRotation.eulerAngles);
            velocityMeasures.Dequeue();
            var velocity = velocityMeasures.Aggregate((v1, v2) => v1 + v2) / velocityMeasures.Count();
            float accelerationX = Mathf.Lerp(-maxAngularForce, maxAngularForce, inputX);
            float accelerationY = Mathf.Lerp(-maxAngularForce, maxAngularForce, inputY);
            float accelerationZ = Mathf.Lerp(-maxAngularForce, maxAngularForce, inputZ);
            joint.targetAngularVelocity = velocity * Time.deltaTime + Time.fixedDeltaTime * new Vector3(accelerationX, accelerationY, accelerationZ);
            lastRotation = joint.GetJointRotation(startRotation);
        }
        int GetSetBitCount(long lValue) {
            int iCount = 0;
            while (lValue != 0) {
                lValue = lValue & (lValue - 1);
                iCount++;
            }
            return iCount;
        }
    }
}
