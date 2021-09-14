using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace MLBoxing.Ragdoll {
    [RequireComponent(typeof(ArticulationBody))]
    public class ArticulationController : MonoBehaviour {
        [SerializeField]
        public LimbType limbType = default;
        [SerializeField]
        public JointType jointType = default;
        [SerializeField]
        float siffness = 1000;
        [SerializeField]
        float damping = 1000;
        [SerializeField]
        bool useAxis = false;
        [SerializeField]
        Vector3 primaryAxis = Vector3.right;
        [SerializeField]
        Vector3 secondaryAxis = Vector3.down;
        [SerializeField]
        public DOF dof = default;
        [HideInInspector, Range(0, 1)]
        public float inputX = 0;
        [HideInInspector, Range(0, 1)]
        public float inputY = 0;
        [HideInInspector, Range(0, 1)]
        public float inputZ = 0;
        [HideInInspector]
        public ArticulationBody articulation;

        public float mass => articulation.mass;
        public Vector3 position => articulation.worldCenterOfMass;
        public float deltaTorque => torque - lastTorque;
        public float currentTorque => torque;
        public int dofCount => articulation.dofCount;

        float torque = 0;
        float lastTorque = 0;

        ArticulationReducedSpace positionBackup;
        ArticulationReducedSpace velocityBackup;
        ArticulationReducedSpace forceBackup;
        ArticulationReducedSpace accelerationBackup;
        Vector3 backupPosition;
        Quaternion backupRotation;
        float backupFriction;

        private void OnValidate() {
            if (articulation == null) {
                articulation = GetComponent<ArticulationBody>();              
            }
            articulation.anchorPosition = Vector3.zero;
            CalculateDOF();
            SetDriveSettings();
            if (useAxis) {
                var right = primaryAxis;
                var forward = Vector3.Cross(primaryAxis, secondaryAxis).normalized;
                var up = Vector3.Cross(forward, right).normalized;
                articulation.anchorRotation = Quaternion.LookRotation(forward, up);
            }
        }

        private void Awake() {
            CalculateDOF();
            SnapshotArticulation();
        }

        private void Start() {
            SetInitialInputs();
        }

        private void FixedUpdate() {
            lastTorque = torque;
            torque = CalculateCurrentTorque();
        }

        public void SetInputX(float value) {
            inputX = value;
            var xDrive = articulation.xDrive;
            xDrive.target = Mathf.Lerp(xDrive.lowerLimit, xDrive.upperLimit, inputX);
            articulation.xDrive = xDrive;
        }
        public void SetInputY(float value) {
            inputY = value;
            var yDrive = articulation.yDrive;
            yDrive.target = Mathf.Lerp(yDrive.lowerLimit, yDrive.upperLimit, inputY);
            articulation.yDrive = yDrive;
        }
        public void SetInputZ(float value) {
            inputZ = value;
            var zDrive = articulation.zDrive;
            zDrive.target = Mathf.Lerp(zDrive.lowerLimit, zDrive.upperLimit, inputZ);
            articulation.zDrive = zDrive;
        }

        public float GetNormalizedJointPosition(DOF axis) {
            float reducedJointPosition = GetJointPosition(axis);
            var drive = axis switch
            {
                DOF.X => articulation.xDrive,
                DOF.Y => articulation.yDrive,
                DOF.Z => articulation.zDrive,
                _ => throw new Exception("Invalid Joint Axis")
            };
            float lowerLimit = drive.lowerLimit;
            float upperLimit = drive.upperLimit;
            return Mathf.InverseLerp(lowerLimit, upperLimit, reducedJointPosition);
        }

        public void ResetArticulation() {
            RestoreArticulation();
            SetInitialInputs();
        }

        void RestoreArticulation() {
            articulation.Sleep();
            if (articulation.isRoot) {
                articulation.TeleportRoot(backupPosition, backupRotation);
            }
            articulation.ResetCenterOfMass();
            articulation.ResetInertiaTensor();
            articulation.jointPosition = positionBackup;
            articulation.jointVelocity = velocityBackup;
            articulation.jointForce = forceBackup;
            articulation.jointAcceleration = accelerationBackup;
            articulation.jointFriction = backupFriction;
            torque = 0;
            lastTorque = 0;
            articulation.WakeUp();
        }

        void SnapshotArticulation() {
            positionBackup = articulation.jointPosition;
            velocityBackup = articulation.jointVelocity;
            forceBackup = articulation.jointForce;
            backupFriction = articulation.jointFriction;
            accelerationBackup = articulation.jointAcceleration;
            backupPosition = transform.position;
            backupRotation = transform.rotation;
        }


        private void SetInitialInputs() {
            SetInputX(GetNormalizedJointPosition(DOF.X));
            SetInputY(GetNormalizedJointPosition(DOF.Y));
            SetInputZ(GetNormalizedJointPosition(DOF.Z));
        }

        void SetDriveSettings() {
            var xDrive = articulation.xDrive;
            var yDrive = articulation.yDrive;
            var zDrive = articulation.zDrive;
            xDrive.stiffness = siffness;
            xDrive.damping = damping;
            yDrive.stiffness = siffness;
            yDrive.damping = damping;
            zDrive.stiffness = siffness;
            zDrive.damping = damping;
            articulation.xDrive = xDrive;
            articulation.yDrive = yDrive;
            articulation.zDrive = zDrive;
         }


        float GetJointPosition(DOF axis) {
            int i = 0;
            foreach (Enum value in Enum.GetValues(typeof(DOF))) {
                if (dof.HasFlag(value)) {
                    if (axis == (DOF)value) {
                        if (articulation.jointPosition.dofCount > i) {
                            return articulation.jointPosition[i];
                        }
                    }
                    i++;
                }
            }
            return 0;
        }

        private float CalculateCurrentTorque() {
            var space = articulation.jointForce;
            float sum = 0;
            for (int i = 0; i < space.dofCount; i++) {
                sum += Mathf.Pow(space[i], 2);
            }
            return Mathf.Sqrt(sum);
        }

        void CalculateDOF() {
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
        }
    }
}