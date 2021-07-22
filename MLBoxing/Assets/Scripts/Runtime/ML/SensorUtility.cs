using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLBoxing.Ragdoll;

namespace MLBoxing.ML {
    public class SensorUtility {
        public static Vector3 GetSubjectivePosition(RagdollModel context, Vector3 positionWorldSpace) {
            var positionDelta = positionWorldSpace - context.root.position;
            var inverseRotation = Quaternion.Inverse(context.root.transform.rotation);
            return inverseRotation * positionDelta;
        }

        public static Vector3 GetNormalizedSubjectivePosition(RagdollModel context, Vector3 positionWorldSpace, float range) {
            var subjectivePosition = GetSubjectivePosition(context, positionWorldSpace);
            var clampedPosition = Vector3.ClampMagnitude(subjectivePosition, range);
            return clampedPosition / range;
        }
    }
}
