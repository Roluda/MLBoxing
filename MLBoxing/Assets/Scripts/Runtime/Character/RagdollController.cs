using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MLBoxing.Character {
    public class RagdollController : MonoBehaviour {

        [SerializeField]
        RagdollLimb[] limbs = default;
        [SerializeField]
        RagdollJoint[] joints = default;

        public IEnumerable<RagdollLimb> allLimbs => limbs.AsEnumerable();
        public IEnumerable<RagdollJoint> allJoints => joints.AsEnumerable();

        public IEnumerable<RagdollLimb> FilterLimbs(LimbType flag) {
            return allLimbs.Where(limb => flag.HasFlag(limb.limbType));
        }

        public IEnumerable<RagdollJoint> FilterJoints(JointType flag) {
            return allJoints.Where(joint => flag.HasFlag(joint.jointType));
        }

        private void OnValidate() {
            limbs = GetComponentsInChildren<RagdollLimb>();
            joints = GetComponentsInChildren<RagdollJoint>();
        }
    }
}
