using System;
using UnityEngine;

namespace MLBoxing.Ragdoll {
    public class Hitbox : MonoBehaviour {
        Collider attachedCollider;
        public RagdollModel owner;
        public Action<float> onHit;

        private void OnValidate() {
            if (!attachedCollider) {
                attachedCollider = GetComponent<Collider>();
            }
            if (!owner) {
                owner = GetComponentInParent<RagdollModel>();
            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.collider.TryGetComponent<Hurtbox>(out var hurtbox)) {
                if (hurtbox.EligibleHit(this)) {
                    if (hurtbox.Hurt(collision.impulse.magnitude, out float damage)) {
                        onHit?.Invoke(damage);
                    }
                }
            }
        }
    }
}
