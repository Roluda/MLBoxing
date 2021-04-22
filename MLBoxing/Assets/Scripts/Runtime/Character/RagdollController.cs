using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.Character {
    public class RagdollController : MonoBehaviour {
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


        // Update is called once per frame
        void FixedUpdate() {
            SetAllTargetRotations();
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
    }
}
