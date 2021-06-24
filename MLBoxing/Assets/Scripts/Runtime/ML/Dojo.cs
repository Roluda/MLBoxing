using System;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

namespace MLBoxing.ML {
    public class Dojo : MonoBehaviour {

        [Header("Spacial Settings")]
        [SerializeField]
        Vector2 gridSize = default;
        [SerializeField, Range(1, 100)]
        float gridSpace = 10;
        [SerializeField]
        Arena[] arenas = default;

        [Header("Lesson Settings")]
        [SerializeField]
        string lessonParameter = "lesson";
        [SerializeField]
        Lesson[] lessons = default;

        [SerializeField]
        int currentLesson = 0;

        List<Arena> currentArenas = new List<Arena>();

        // Start is called before the first frame update
        void Start() {
            InitArenas();
        }

        private void Update() {
            CheckLessonParameter();
        }

        private void CheckLessonParameter() {
            if(Mathf.RoundToInt(Academy.Instance.EnvironmentParameters.GetWithDefault(lessonParameter, 0)) > currentLesson) {
                currentLesson++;
                currentArenas.ForEach(arena => arena.SetLesson(lessons[currentLesson]));
            }
        }

        void InitArenas() {
            for (int x = 0; x < gridSize.x; x++) {
                for (int y = 0; y < gridSize.y; y++) {
                    var position = new Vector3(x, 0, y) * gridSpace;
                    var arena = Instantiate(arenas[UnityEngine.Random.Range(0, arenas.Length)], position, Quaternion.identity, transform);
                    arena.SetLesson(lessons[currentLesson]);
                    currentArenas.Add(arena);
                }
            }
        }
    }
}
