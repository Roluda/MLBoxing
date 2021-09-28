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
        public SpawnPoint[] possibleSpawnPoints = default;

        int steps = 0;
        List<ModularAgent> currentAgents = new List<ModularAgent>();
        List<SpawnPoint> occupiedSpawnPoints = new List<SpawnPoint>();

        void OnEnable() {
            Academy.Instance.OnEnvironmentReset += EndEpisode;
        }

        private void FixedUpdate() {
            UpdateEpisodeTime();
        }

        void UpdateEpisodeTime() {
            if(lesson.episodeLength > 0) {
                steps++;
                if (steps >= lesson.episodeLength) {
                    EndEpisode();
                }
            }
        }

        public void StartLesson(Lesson lesson) {
            if (this.lesson) {
                UnregisterRewards();
                UnregisterTerminators();
                currentAgents.ForEach(agent => agent.EpisodeInterrupted());
                currentAgents.ForEach(agent => Destroy(agent.gameObject));
                currentAgents.Clear();
            }
            this.lesson = lesson;
            SpawnAgents();
            RegisterRewards();
            RegisterTerminators();
            steps = 0;
        }
        void SpawnAgents() {
            Assert.IsFalse(lesson.student.enabled, "Agent has to be disabled on Initialize to prevent OnEnable");
            occupiedSpawnPoints.Clear();
            var firstAgent = Instantiate(lesson.student, transform);
            firstAgent.spawnPoint = NextRandomSpawn();
            firstAgent.onTerminated += (agent) => EndEpisode();
            currentAgents.Add(firstAgent);
            if (lesson.mirrorOpponent || lesson.selfPlay) {
                Assert.IsTrue(possibleSpawnPoints.Length > 1, "Not enough Spawn Points for self Play");
                var secondAgent = Instantiate(lesson.student, transform);
                secondAgent.spawnPoint = NextRandomSpawn();
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

        private void EndEpisode() {
            if (lesson.selfPlay) {
                DetermineWinner();
            }
            currentAgents.ForEach(agent => agent.EndEpisode());
            steps = 0;
        }

        private void DetermineWinner() {
            if (currentAgents.Count < 2) {
                return;
            }
            if (currentAgents[0].score > currentAgents[1].score) {
                currentAgents[0].Win();
                currentAgents[1].Lose();
            }else if (currentAgents[0].score < currentAgents[1].score) {
                currentAgents[1].Win();
                currentAgents[0].Lose();
            }
        }

        private void RegisterRewards() {
            currentAgents.ForEach(agent => lesson.rewards.ForEach(reward => reward.AddRewardListeners(agent)));
        }

        private void UnregisterRewards() {
            currentAgents.ForEach(agent => lesson.rewards.ForEach(reward => reward.RemoveRewardListeners(agent)));
        }

        private void RegisterTerminators() {
            currentAgents.ForEach(agent => lesson.terminaters.ForEach(terminater => terminater.AddTerminationListeners(agent)));
        }

        private void UnregisterTerminators() {
            currentAgents.ForEach(agent => lesson.terminaters.ForEach(terminater => terminater.RemoveTerminationListeners(agent)));
        }

        SpawnPoint NextRandomSpawn() {
            var availableSpawns = possibleSpawnPoints.Except(occupiedSpawnPoints).ToArray();
            var randomSpawn = availableSpawns[UnityEngine.Random.Range(0, availableSpawns.Length)];
            occupiedSpawnPoints.Add(randomSpawn);
            return randomSpawn;
        }

        private void OnTriggerExit(Collider other) {
            var agent = other.GetComponentInParent<ModularAgent>();
            if (agent) {
                agent.Terminate();
            }
        }
    }
}
