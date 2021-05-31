using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.Character {
    public class RagdollController : MonoBehaviour {
        [Header("Rigidbodies")]
        [SerializeField]
        Rigidbody hips = default;
        [SerializeField]
        Rigidbody torso = default;
        [SerializeField]
        Rigidbody head = default;
        [SerializeField]
        Rigidbody leftUpperArm = default;
        [SerializeField]
        Rigidbody leftForearm = default;
        [SerializeField]
        Rigidbody rightUpperArm = default;
        [SerializeField]
        Rigidbody rightForearm = default;
        [SerializeField]
        Rigidbody leftThigh = default;
        [SerializeField]
        Rigidbody leftShin = default;
        [SerializeField]
        Rigidbody rightThigh = default;
        [SerializeField]
        Rigidbody rightShin = default;

        [Header("Joints")]
        [SerializeField]
        ConfigurableJoint neck = default;
        [SerializeField]
        ConfigurableJoint chest = default;
        [SerializeField]
        ConfigurableJoint leftShoulder = default;
        [SerializeField]
        ConfigurableJoint leftElbow = default;
        [SerializeField]
        ConfigurableJoint rightShoulder = default;
        [SerializeField]
        ConfigurableJoint rightElbow = default;
        [SerializeField]
        ConfigurableJoint leftHip = default;
        [SerializeField]
        ConfigurableJoint leftKnee = default;
        [SerializeField]
        ConfigurableJoint rightHip = default;
        [SerializeField]
        ConfigurableJoint rightKnee = default;

        public float neckX { get; set; }
        public float neckY { get; set; }

        public float chestX { get; set; }
        public float chestY { get; set; }

        public float leftShoulderX { get; set; }
        public float leftShoulderY { get; set; }
        public float leftElbowX { get; set; }

        public float rightShoulderX { get; set; }
        public float rightShoulderY { get; set; }
        public float rightElbowX { get; set; }

        public float leftHipX { get; set; }
        public float leftHipY { get; set; }
        public float leftKneeX { get; set; }

        public float rightHipX { get; set; }
        public float rightHipY { get; set; }
        public float rightKneeX { get; set; }

        public IEnumerable<ConfigurableJoint> allJoints {
            get {
                yield return neck;
                yield return chest;
                yield return leftShoulder;
                yield return leftElbow;
                yield return rightShoulder;
                yield return rightElbow;
                yield return leftHip;
                yield return leftKnee;
                yield return rightHip;
                yield return rightKnee;
            }
        }

        public IEnumerable<Rigidbody> allRigidbodies {
            get {
                yield return hips;
                yield return torso;
                yield return head;
                yield return leftUpperArm;
                yield return leftForearm;
                yield return rightUpperArm;
                yield return rightForearm;
                yield return leftThigh;
                yield return leftShin;
                yield return rightThigh;
                yield return rightShin;
            }
        }

        Dictionary<ConfigurableJoint, Quaternion> startRotations = new Dictionary<ConfigurableJoint, Quaternion>();

        private void Awake() {
            CacheStartRotations();
            SetInitialValues();
        }

        private void CacheStartRotations() {
            foreach(var joint in allJoints) {
                startRotations[joint] = joint.transform.rotation;
            }
        }

        void FixedUpdate() {
            SetAllTargetRotations();
        }

        private void SetInitialValues() {
            neckX = GetNormalizedJointRotationX(neck);
            neckY = GetNormalizedJointRotationY(neck);

            chestX = GetNormalizedJointRotationX(chest);
            chestY = GetNormalizedJointRotationY(chest);

            leftShoulderX = GetNormalizedJointRotationX(leftShoulder);
            leftShoulderY = GetNormalizedJointRotationY(leftShoulder);
            leftElbowX = GetNormalizedJointRotationX(leftElbow);

            rightShoulderX = GetNormalizedJointRotationX(rightShoulder);
            rightShoulderY = GetNormalizedJointRotationY(rightShoulder);
            rightElbowX = GetNormalizedJointRotationX(rightElbow);

            leftHipX = GetNormalizedJointRotationX(leftHip);
            leftHipY = GetNormalizedJointRotationY(leftHip);
            leftKneeX = GetNormalizedJointRotationX(leftKnee);

            rightHipX = GetNormalizedJointRotationX(rightHip);
            rightHipY = GetNormalizedJointRotationY(rightHip);
            rightKneeX = GetNormalizedJointRotationX(rightKnee);
        }

        void SetAllTargetRotations() {
            SetTargetRotation(neck, neckX, neckY);
            SetTargetRotation(chest, chestX, chestY);

            SetTargetRotation(leftShoulder, leftShoulderX, leftShoulderY);
            SetTargetRotation(leftElbow, leftElbowX, 0);

            SetTargetRotation(rightShoulder, rightShoulderX, rightShoulderY);
            SetTargetRotation(rightElbow, rightElbowX, 0);

            SetTargetRotation(leftHip, leftHipX, leftHipY);
            SetTargetRotation(leftKnee, leftKneeX, 0);

            SetTargetRotation(rightHip, rightHipX, rightHipY);
            SetTargetRotation(rightKnee, rightKneeX, 0);
        }

        void SetTargetRotation(ConfigurableJoint joint, float normalizedX, float normalizedY) {
            normalizedX = Mathf.Clamp(normalizedX, 0, 1);
            normalizedY = Mathf.Clamp(normalizedY, 0, 1);
            float minX = joint.lowAngularXLimit.limit;
            float maxX = joint.highAngularXLimit.limit;
            float minY = -joint.angularYLimit.limit;
            float maxY = joint.angularYLimit.limit;
            float targetX = minX + normalizedX * (maxX - minX);
            float targetY = minY + normalizedY * (maxY - minY);
            joint.targetRotation = Quaternion.Euler(targetX, targetY, 0);
        }


        public float GetNormalizedJointRotationX(ConfigurableJoint joint) {
            float min = joint.lowAngularXLimit.limit;
            float max = joint.highAngularXLimit.limit;
            float current = joint.GetJointRotation(startRotations[joint]).eulerAngles.x;
            return (current - min) / (max - min);
        }

        public float GetNormalizedJointRotationY(ConfigurableJoint joint) {
            float min = joint.angularYLimit.limit;
            float max = -joint.angularYLimit.limit;
            float current = joint.GetJointRotation(startRotations[joint]).eulerAngles.y;
            return (current - min) / (max - min);
        }

        public Quaternion GetJointRotation(ConfigurableJoint joint) {
            var right = joint.axis;
            var forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
            var up = Vector3.Cross(forward, right).normalized;
            return Quaternion.FromToRotation(up, joint.connectedBody.transform.rotation.eulerAngles);
        }
    }
}
