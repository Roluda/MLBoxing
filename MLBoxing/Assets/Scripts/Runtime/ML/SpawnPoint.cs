using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLBoxing.ML {
    public class SpawnPoint : MonoBehaviour {

        [SerializeField]
        float spawnRadius = 3;
        [SerializeField, Range(0, 180)]
        float randomAngle = 360;

        public Vector3 RandomPosition() {
            var randomPoint = Random.insideUnitCircle * spawnRadius;
            return transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);
        }

        public Quaternion RandomRotation() {
            return transform.rotation * Quaternion.Euler(0, Random.Range(-randomAngle, randomAngle), 0);
        }
    }
}
