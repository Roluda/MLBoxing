using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.Ragdoll.Features {
    public class HurtParticles : MonoBehaviour {

        [SerializeField]
        ParticleSystem system = default;
        [SerializeField]
        Hurtbox observedHurtbox = default;
        [SerializeField]
        float amountMultiplier = default;

        private void OnValidate() {
            if (!system) {
                system = GetComponent<ParticleSystem>();
            }
        }

        private void OnEnable() {
            observedHurtbox.onHurt += EmitParticles;
        }

        private void OnDisable() {
            observedHurtbox.onHurt -= EmitParticles;
        }

        void EmitParticles(float damage) {
            system.Emit((int)(damage * amountMultiplier));
        }
    }
}
