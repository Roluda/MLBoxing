using System;
using UnityEngine;

namespace MLBoxing.Ragdoll {
    public class Hurtbox : MonoBehaviour {

        [SerializeField]
        DamageCalculator damageCalculator = default;

        public Action<float> onHurt;

        Collider attachedCollider;
        public RagdollModel owner;


        private void OnValidate() {
            if (!attachedCollider) {
                attachedCollider = GetComponent<Collider>();
            }
            if (!owner) {
                owner = GetComponentInParent<RagdollModel>();
            }
        }

        public bool Hurt(float impulse, out float damage) {
            if (damageCalculator.TryCalculateDamage(impulse, out damage)) {
                onHurt?.Invoke(damage);
                return true;
            } else {
                damage = 0;
                return false;
            }
        }

        public bool EligibleHit(Hitbox hitbox) {
            return hitbox.owner != owner;
        }
    }
}
