using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MLBoxing.Ragdoll {
    [RequireComponent(typeof(ArticulationBody))]
    public class ArticulationController : MonoBehaviour {
        [SerializeField]
        public ArticulationBody articulation = default;
        [SerializeField]
        public LimbType limbType = default;
        [SerializeField]
        public JointType jointType = default;
        [SerializeField]
        ControlType control = ControlType.PositionControl;
        [SerializeField]
        float stiffness = 1000;
        [SerializeField]
        float damping = 1000;
        [SerializeField]
        float maxEffect = 1000;


        public float mass => articulation.mass;
        [SerializeField]
        public DOF dof = default;
        [HideInInspector]
        public int dofCount = 1;
        [SerializeField, Range(0, 1)]
        public float inputX = 0;
        [SerializeField, Range(0, 1)]
        public float inputY = 0;
        [SerializeField, Range(0, 1)]
        public float inputZ = 0;
        [SerializeField]
        Vector3 primaryAxis;
        [SerializeField]
        Vector3 secondaryAxis;


        public Vector3 position => articulation.worldCenterOfMass;

        [HideInInspector]
        public float currentTorque = 0;
        public float deltaTorque => currentTorque - lastTorque;

        float lastTorque = 0;

        [SerializeField]
        bool useAxis = false;


        private void OnValidate() {

            if (!articulation) {
                articulation = GetComponent<ArticulationBody>();              
            }
            if (!articulation.isRoot) {
                articulation.jointType = ArticulationJointType.SphericalJoint;
            }
            dof = 0;
            if (articulation.twistLock != ArticulationDofLock.LockedMotion) {
                dof |= DOF.X;
            }
            if (articulation.swingYLock != ArticulationDofLock.LockedMotion) {
                dof |= DOF.Y;
            }
            if (articulation.swingZLock != ArticulationDofLock.LockedMotion) {
                dof |= DOF.Z;
            }
            if (articulation.isRoot) {
                dof = 0;
            }
            dofCount = articulation.dofCount;
            articulation.anchorPosition = Vector3.zero;
            if (useAxis) {
                var right = primaryAxis;
                var forward = Vector3.Cross(primaryAxis, secondaryAxis).normalized;
                var up = Vector3.Cross(forward, right).normalized;
                articulation.anchorRotation = Quaternion.LookRotation(forward, up);
            }

            EnterControlMode(control);
        }

        private void Start() {
            SetInitialInputs();
        }

        private void FixedUpdate() {
            lastTorque = currentTorque;
            currentTorque = CalculateCurrentTorque();
            if (articulation.isRoot) {
                return;
            }
            switch (control) {
                case ControlType.PositionControl:
                    SetTargetRotation();
                    break;
                case ControlType.VelocityControl:
                    SetTargetVelocity();
                    break;
                case ControlType.AccelerationControl:
                    SetTargetForce();
                    break;
                default:
                    throw new NotImplementedException($"control of type {control} is not implemented");
            }
        }

        private void SetInitialInputs() {
            switch (control) {
                case ControlType.PositionControl:
                    inputX = GetNormalizedJointRotation(DOF.X);
                    inputY = GetNormalizedJointRotation(DOF.Y);
                    inputZ = GetNormalizedJointRotation(DOF.Z);
                    break;
                case ControlType.VelocityControl:
                    inputX = 0.5f;
                    inputY = 0.5f;
                    inputZ = 0.5f;
                    break;
                case ControlType.AccelerationControl:
                    inputX = 0.5f;
                    inputY = 0.5f;
                    inputZ = 0.5f;
                    break;
            }
        }

        public void EnterControlMode(ControlType controlType) {
            control = controlType;
            var xDrive = articulation.xDrive;
            var yDrive = articulation.yDrive;
            var zDrive = articulation.zDrive;
            switch (controlType) {
                case ControlType.PositionControl:
                    xDrive.stiffness = stiffness;
                    xDrive.damping = 0;
                    yDrive.stiffness = stiffness;
                    yDrive.damping = 0;
                    zDrive.stiffness = stiffness;
                    zDrive.damping = 0;
                    break;
                case ControlType.VelocityControl:
                    xDrive.stiffness = 0;
                    xDrive.damping = damping;
                    yDrive.stiffness = 0;
                    yDrive.damping = damping;
                    zDrive.stiffness = 0;
                    zDrive.damping = damping;
                    break;
                case ControlType.AccelerationControl:
                    xDrive.stiffness = 0;
                    xDrive.damping = 0;
                    yDrive.stiffness = 0;
                    yDrive.damping = 0;
                    zDrive.stiffness = 0;
                    zDrive.damping = 0;
                    break;
            }
            articulation.xDrive = xDrive;
            articulation.yDrive = yDrive;
            articulation.zDrive = zDrive;
        }

        void SetTargetRotation() {
            var xDrive = articulation.xDrive;
            var yDrive = articulation.yDrive;
            var zDrive = articulation.zDrive;
            xDrive.target = Mathf.Lerp(xDrive.lowerLimit, xDrive.upperLimit, inputX);
            yDrive.target = Mathf.Lerp(yDrive.lowerLimit, yDrive.upperLimit, inputY);
            zDrive.target = Mathf.Lerp(zDrive.lowerLimit, zDrive.upperLimit, inputZ);
            articulation.xDrive = xDrive;
            articulation.yDrive = yDrive;
            articulation.zDrive = zDrive;
        }

        void SetTargetVelocity() {
            var xDrive = articulation.xDrive;
            var yDrive = articulation.yDrive;
            var zDrive = articulation.zDrive;
            xDrive.targetVelocity = Mathf.Lerp(xDrive.lowerLimit, xDrive.upperLimit, inputX);
            yDrive.targetVelocity = Mathf.Lerp(yDrive.lowerLimit, yDrive.upperLimit, inputY);
            zDrive.targetVelocity = Mathf.Lerp(zDrive.lowerLimit, zDrive.upperLimit, inputZ);
            articulation.xDrive = xDrive;
            articulation.yDrive = yDrive;
            articulation.zDrive = zDrive;
        }

        /*
        target Position = ((force + PositionDamper * Velocity) / PositionSpring) + position
        */

        void SetTargetForce() {
            var articulationSpace = new ArticulationReducedSpace() {
                dofCount = dofCount
            };
            int i = 0;
            if (dof.HasFlag(DOF.X))
                articulationSpace[i++] = Mathf.Lerp(-maxEffect, maxEffect, inputX);
            if (dof.HasFlag(DOF.Y))
                articulationSpace[i++] = Mathf.Lerp(-maxEffect, maxEffect, inputZ);
            if (dof.HasFlag(DOF.Z))
                articulationSpace[i++] = Mathf.Lerp(-maxEffect, maxEffect, inputZ);

            articulation.jointForce = articulationSpace;
        }

        public float GetNormalizedJointRotation(DOF axis) {
            float reducedJointPosition = 0;
            int i = 0;
            foreach(Enum value in Enum.GetValues(typeof(DOF))) {
                if (dof.HasFlag(value)) {
                    if (axis == (DOF)value) {
                        reducedJointPosition = articulation.jointPosition[i];
                    }
                    i++;
                }
            }
            var drive = axis switch
            {
                DOF.X => articulation.xDrive,
                DOF.Y => articulation.yDrive,
                DOF.Z => articulation.zDrive,
                _ => throw new Exception("Invalid Joint Axis")
            };
            float lowerLimit = drive.lowerLimit;
            float upperLimit = drive.upperLimit;
            Debug.Log($"reduced Joint Rotation for axis {axis}: {reducedJointPosition}");
            return Mathf.InverseLerp(lowerLimit, upperLimit, reducedJointPosition);
        }

        private float CalculateCurrentTorque() {
            var space = articulation.jointForce;
            float sum = 0;
            for (int i = 0; i < space.dofCount; i++) {
                sum += Mathf.Pow(space[i], 2);
            }
            return Mathf.Sqrt(sum);
        }
    }
}