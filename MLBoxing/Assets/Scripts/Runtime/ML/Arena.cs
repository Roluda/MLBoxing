using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.Assertions;

namespace MLBoxing.ML {
    public class Arena : MonoBehaviour {
        [Header("Configuration")]
        [SerializeField]
        Lesson lesson = default;
        [SerializeField]
        public Transform[] possibleSpawnPoints = default;


        int steps = 0;
        List<ModularAgent> currentAgents = new List<ModularAgent>();
        List<Transform> occupiedSpawnPoints = new List<Transform>();

        void OnEnable() {
            Academy.Instance.OnEnvironmentReset += ResetArena;
        }

        void OnDisable() {
            Academy.Instance.OnEnvironmentReset -= ResetArena;
        }

        private void FixedUpdate() {
            UpdateEpisodeTime();
        }

        public void SetLesson(Lesson lesson) {
            this.lesson = lesson;
            ResetArena();
        }

        public void UpdateEpisodeTime() {
            if(lesson.episodeLength > 0) {
                steps++;
                if (steps >= lesson.episodeLength) {
                    EndEpisode();
                }
            }
        }

        private void EndEpisode() {
            if (lesson.selfPlay) {
                DetermineWinner();
            }
            ResetArena();
        }

        private void DetermineWinner() {
            if (currentAgents[0].score > currentAgents[1].score) {
                currentAgents[0].Win();
                currentAgents[1].Lose();
            }else if (currentAgents[0].score < currentAgents[1].score) {
                currentAgents[1].Win();
                currentAgents[0].Lose();
            }
        }

        private void ResetArena() {
            KillCurrentAgents();
            SpawnNewAgents();
            RegisterRewards();
            RegisterTerminaters();
            steps = 0;
        }

        void KillCurrentAgents() {
            currentAgents.ForEach(agent => agent.Kill());
            currentAgents.Clear();
        }

        void SpawnNewAgents() {
            Assert.IsFalse(lesson.student.enabled, "Agent has to be disabled on Initialize to prevent OnEnable");
            occupiedSpawnPoints.Clear();
            var nextSpawn = NextRandomSpawn();
            var firstAgent = Instantiate(lesson.student, nextSpawn.position , nextSpawn.rotation, transform);
            firstAgent.onTerminated += (agent) => EndEpisode();
            currentAgents.Add(firstAgent);
            if (lesson.mirrorOpponent || lesson.selfPlay) {
                Assert.IsTrue(possibleSpawnPoints.Length > 1, "Not enough Spawn Points for self Play");
                nextSpawn = NextRandomSpawn();
                var secondAgent = Instantiate(lesson.student, nextSpawn.position, nextSpawn.rotation, transform);
                secondAgent.onTerminated += (agent) => EndEpisode();
                currentAgents.Add(secondAgent);
                firstAgent.SetOpponent(secondAgent);
                secondAgent.SetOpponent(firstAgent);
                if (lesson.selfPlay) {
                    secondAgent.team = firstAgent.team + 1;
                }
                secondAgent.enabled = true;
            }
            firstAgent.enabled = true;
        }

        private void RegisterRewards() {
            currentAgents.ForEach(agent => lesson.rewards.ForEach(reward => reward.AddRewardListeners(agent)));
        }

        private void RegisterTerminaters() {
            currentAgents.ForEach(agent => lesson.terminaters.ForEach(terminater => terminater.AddTerminationListeners(agent)));
        }

        Transform NextRandomSpawn() {
            var availableSpawns = possibleSpawnPoints.Except(occupiedSpawnPoints).ToArray();
            var randomSpawn = availableSpawns[UnityEngine.Random.Range(0, availableSpawns.Length)];
            occupiedSpawnPoints.Add(randomSpawn);
            return randomSpawn;
        }
    }
}
