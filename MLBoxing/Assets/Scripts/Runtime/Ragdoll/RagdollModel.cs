using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MLBoxing.Ragdoll {
    public class RagdollModel : MonoBehaviour {
        [Header("Configuration")]
        [SerializeField]
        public Rigidbody root = default;
        [SerializeField]
        public float height = 1.7f;
        [SerializeField]
        public float span = 1.7f;
        [SerializeField]
        RagdollLimb[] limbs = default;
        [SerializeField]
        RagdollJoint[] joints = default;
        [SerializeField]
        Hurtbox[] hurtboxes = default;
        [SerializeField]
        Hitbox[] hitboxes = default;
        [SerializeField, EnumFlags]
        RigidbodyConstraints rootConstraints = default;

        [SerializeField]
        bool applyControlTypeToAllJoints = true;
        [SerializeField]
        ControlType controlType = ControlType.PositionControl;

        public IEnumerable<RagdollLimb> allLimbs => limbs.AsEnumerable();
        public IEnumerable<RagdollJoint> allJoints => joints.AsEnumerable();
        public IEnumerable<Hitbox> allHitboxes => hitboxes.AsEnumerable();
        public IEnumerable<Hurtbox> allHurtboxes => hurtboxes.AsEnumerable();

        public IEnumerable<RagdollLimb> FilterLimbs(LimbType flag) {
            return allLimbs.Where(limb => flag.HasFlag(limb.limbType));
        }
        public IEnumerable<RagdollJoint> FilterJoints(JointType flag) {
            return allJoints.Where(joint => flag.HasFlag(joint.jointType));
        }
        public RagdollLimb GetLimb(LimbType type) {
            return FilterLimbs(type).First();
        }
        public RagdollJoint GetJoint(JointType type) {
            return FilterJoints(type).First();
        }

        private void OnValidate() {
            limbs = GetComponentsInChildren<RagdollLimb>();
            joints = GetComponentsInChildren<RagdollJoint>();
            hurtboxes = GetComponentsInChildren<Hurtbox>();
            hitboxes = GetComponentsInChildren<Hitbox>();
            root = FilterLimbs(LimbType.Hips).First().limb;
            root.constraints = rootConstraints;
            if (applyControlTypeToAllJoints) {
                foreach( var joint in joints) {
                    joint.EnterControlMode(controlType);
                }
            }
        }

        private void Awake() {
            foreach(var hitbox in allHitboxes) {
                hitbox.onHit += (damage) => totalDamageDealt += damage;
            }
            foreach(var hurtbox in allHurtboxes) {
                hurtbox.onHurt += (damage) => totalDamageTaken += damage;
            }
            height = (GetLimb(LimbType.Head).limb.position - GetLimb(LimbType.LeftFoot).limb.position).magnitude;
            span = (GetLimb(LimbType.LeftPalm).limb.position - GetLimb(LimbType.RightPalm).limb.position).magnitude;
        }
        public float totalDamageTaken;
        public float totalDamageDealt;
    }
}
