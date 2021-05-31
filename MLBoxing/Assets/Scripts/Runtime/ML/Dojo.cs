using UnityEngine;

namespace MLBoxing.ML {
    public class Dojo : MonoBehaviour {

        [Header("Spacial Settings")]
        [SerializeField]
        Vector2 gridSize = default;
        [SerializeField, Range(1, 100)]
        float gridSpace = 10;
        [SerializeField]
        Arena[] arenaPrefabs = default;
        [SerializeField]
        ModularAgent agentToTrain = default;

        // Start is called before the first frame update
        void Start() {
            InitArenas();
        }

        void InitArenas() {
            for (int x = 0; x < gridSize.x; x++) {
                for (int y = 0; y < gridSize.y; y++) {
                    var position = new Vector3(x, 0, y) * gridSpace;
                    var arena = Instantiate(arenaPrefabs[Random.Range(0, arenaPrefabs.Length)], position, Quaternion.identity, transform);
                    if (agentToTrain != null) {
                        arena.SetAgentPrefab(agentToTrain);
                    }
                }
            }
        }
    }
}
