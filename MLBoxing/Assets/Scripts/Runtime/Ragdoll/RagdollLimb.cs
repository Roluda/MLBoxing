using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.Ragdoll {
    public class RagdollLimb : MonoBehaviour {
        [SerializeField]
        public Rigidbody limb = default;
        [SerializeField]
        public LimbType limbType = default;

        public float mass => limb.mass;
        public Vector3 position => limb.position;


        private void OnValidate() {
            if (!limb) {
                limb = GetComponent<Rigidbody>();
            }
        }
    }
}
