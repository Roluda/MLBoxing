using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.Character { 
public class RagdollJoint : MonoBehaviour
{
        [SerializeField]
        public ConfigurableJoint joint = default;
        [SerializeField]
        public JointType jointType = default;
        [SerializeField, Range(0,1)]
        public float positionInputX = 0;
        [SerializeField, Range(0,1)]
        public float positionInputY = 0;


        Quaternion startRotation;



        private void OnValidate() {
            if (!joint) {
                joint = GetComponent<ConfigurableJoint>();
            }
        }

        private void Awake() {
            startRotation = transform.rotation;
            positionInputX = GetNormalizedJointRotationX();
            positionInputY = GetNormalizedJointRotationY();
        }

        private void Update() {
            SetTargetRotation();
        }

        void SetTargetRotation() {
            float minX = joint.lowAngularXLimit.limit;
            float maxX = joint.highAngularXLimit.limit;
            float minY = -joint.angularYLimit.limit;
            float maxY = joint.angularYLimit.limit;
            float targetX = minX + positionInputX * (maxX - minX);
            float targetY = joint.angularYLimit.limit > 0 ? minY + positionInputY * (maxY - minY) : 0;
            joint.targetRotation = Quaternion.Euler(targetX, targetY, 0);
        }


        public float GetNormalizedJointRotationX() {
            float min = joint.lowAngularXLimit.limit;
            float max = joint.highAngularXLimit.limit;
            float current = joint.GetJointRotation(startRotation).eulerAngles.x;
            return (current - min) / (max - min);
        }

        public float GetNormalizedJointRotationY() {
            float min = joint.angularYLimit.limit;
            float max = -joint.angularYLimit.limit;
            float current = joint.GetJointRotation(startRotation).eulerAngles.y;
            return (current - min) / (max - min);
        }

        Quaternion GetJointRotation() {
            var right = joint.axis;
            var forward = Vector3.Cross(joint.axis, joint.secondaryAxis).normalized;
            var up = Vector3.Cross(forward, right).normalized;
            return Quaternion.FromToRotation(up, joint.connectedBody.transform.rotation.eulerAngles);
        }
    }
}
