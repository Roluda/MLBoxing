using UnityEngine;

namespace MLBoxing.Ragdoll {
    [CreateAssetMenu(fileName = "DamageCalulator_New", menuName = "DamageCalculator")]
    public class DamageCalculator : ScriptableObject {

        [SerializeField]
        float impulseThreshold = 100;
        [SerializeField]
        float maximumImpuse = 1000;
        [SerializeField]
        float baseDamage = 1;
        [SerializeField]
        float maximumBonusDamage = 2;
        [SerializeField]
        AnimationCurve bonusOverImpulse;

        public bool TryCalculateDamage(float impulse, out float damage) {
            if (impulse < impulseThreshold) {
                damage = 0;
                return false;
            } else {
                float scaledImpulse = Mathf.InverseLerp(impulseThreshold, maximumImpuse, impulse);
                damage = baseDamage + Mathf.Clamp01(bonusOverImpulse.Evaluate(scaledImpulse)) * maximumBonusDamage;
                return true;
            }
        }
    }
}
