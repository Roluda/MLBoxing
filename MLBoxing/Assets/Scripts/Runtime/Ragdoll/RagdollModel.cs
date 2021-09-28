using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MLBoxing.Ragdoll {
    public class RagdollModel : MonoBehaviour {
        [Header("Configuration")]
        [SerializeField]
        public ArticulationBody root;
        [SerializeField]
        public float height = 1.7f;
        [SerializeField]
        public float span = 1.7f;
        [SerializeField]
        ArticulationController[] articulations;
        [SerializeField]
        Hurtbox[] hurtboxes = default;
        [SerializeField]
        Hitbox[] hitboxes = default;
        [SerializeField]
        int solverIterations = 20;

        public float totalDamageTaken;
        public float totalDamageDealt;

        public Vector3 rootPosition => root.worldCenterOfMass;

        public Vector3 deltaPosition => currentPosition - lastPosition;
        Vector3 currentPosition = Vector3.zero;
        Vector3 lastPosition = Vector3.zero;
        Vector3 rootInitialPosition;
        Quaternion rootInitialRotation;

        public IEnumerable<ArticulationController> allArticulations => articulations.AsEnumerable();
        public IEnumerable<Hitbox> allHitboxes => hitboxes.AsEnumerable();
        public IEnumerable<Hurtbox> allHurtboxes => hurtboxes.AsEnumerable();

        public IEnumerable<ArticulationController> FilterLimbs(LimbType flag) {
            return allArticulations.Where(limb => flag.HasFlag(limb.limbType));
        }
        public IEnumerable<ArticulationController> FilterJoints(JointType flag) {
            return allArticulations.Where(joint => flag.HasFlag(joint.jointType));
        }
        public ArticulationController GetLimb(LimbType type) {
            return FilterLimbs(type).First();
        }
        public ArticulationController GetJoint(JointType type) {
            return FilterJoints(type).First();
        }

        public void ResetModell(Vector3 position, Quaternion rotation) {
            root.Sleep();
            root.TeleportRoot(position+rootInitialPosition, rotation*rootInitialRotation);
            foreach (var arti in articulations) {
                arti.ResetArticulation();
            }
            root.WakeUp();
            totalDamageTaken = 0;
            totalDamageDealt = 0;
        }

        private void OnValidate() {
            articulations = GetComponentsInChildren<ArticulationController>();
            hurtboxes = GetComponentsInChildren<Hurtbox>();
            hitboxes = GetComponentsInChildren<Hitbox>();
            root.solverIterations = solverIterations;
        }

        private void Awake() {
            rootInitialPosition = root.transform.localPosition;
            rootInitialRotation = root.transform.localRotation;
            foreach (var hitbox in allHitboxes) {
                hitbox.onHit += (damage) => totalDamageDealt += damage;
            }
            foreach(var hurtbox in allHurtboxes) {
                hurtbox.onHurt += (damage) => totalDamageTaken += damage;
            }
            height = (GetLimb(LimbType.Head).position - GetLimb(LimbType.LeftFoot).position).magnitude;
            span = (GetLimb(LimbType.LeftPalm).position - GetLimb(LimbType.RightPalm).position).magnitude;
        }

        private void FixedUpdate() {
            lastPosition = currentPosition;
            currentPosition = rootPosition;
        }
    }
}
