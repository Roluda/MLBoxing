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
        ModularAgent agentPrefab = default;
        [SerializeField]
        public bool selfPlay = false;
        [SerializeField]
        List<Reward> rewards = default;
        [SerializeField]
        List<Terminater> terminaters = default;
        [SerializeField]
        Transform[] possibleSpawnPoints = default;


        List<ModularAgent> currentAgents = new List<ModularAgent>();
        List<Transform> occupiedSpawnPoints = new List<Transform>();

        void OnEnable() {
            Academy.Instance.OnEnvironmentReset += ResetArena;
        }

        void OnDisable() {
            Academy.Instance.OnEnvironmentReset -= ResetArena;
        }

        public void SetAgentPrefab(ModularAgent agent) {
            agentPrefab = agent;
        }

        public void SetRewards(Reward[] rewards) {
            this.rewards = rewards.ToList();
        }

        public void SetTerminaters(Terminater[] terminaters) {
            this.terminaters = terminaters.ToList();
        }

        public void ShutDown() {
            KillCurrentAgents();
        }

        private void ResetArena() {
            KillCurrentAgents();
            SpawnNewAgents();
            RegisterRewards();
            RegisterTerminaters();
        }

        void KillCurrentAgents() {
            currentAgents.ForEach(agent => agent.Kill());
            currentAgents.Clear();
        }

        void SpawnNewAgents() {
            Assert.IsFalse(agentPrefab.enabled, "Agent has to be disabled on Initialize to prevent OnEnable");
            occupiedSpawnPoints.Clear();
            var nextSpawn = NextRandomSpawn();
            var firstAgent = Instantiate(agentPrefab, nextSpawn.position , nextSpawn.rotation, transform);
            firstAgent.onTerminated += (agent) => ResetArena();
            currentAgents.Add(firstAgent);
            if (selfPlay) {
                Assert.IsTrue(possibleSpawnPoints.Length > 1, "Not enough Spawn Points for self Play");
                nextSpawn = NextRandomSpawn();
                var secondAgent = Instantiate(agentPrefab, nextSpawn.position, nextSpawn.rotation, transform);
                secondAgent.onTerminated += (agent) => ResetArena();
                currentAgents.Add(secondAgent);
                firstAgent.SetOpponent(secondAgent);
                secondAgent.SetOpponent(firstAgent);
                secondAgent.SetTeam(1);
                secondAgent.enabled = true;
            }
            firstAgent.enabled = true;
        }

        private void RegisterRewards() {
            currentAgents.ForEach(agent => rewards.ForEach(reward => reward.AddRewardListeners(agent)));
        }

        private void RegisterTerminaters() {
            currentAgents.ForEach(agent => terminaters.ForEach(terminater => terminater.AddTerminationListeners(agent)));
        }

        Transform NextRandomSpawn() {
            var availableSpawns = possibleSpawnPoints.Except(occupiedSpawnPoints).ToArray();
            var randomSpawn = availableSpawns[UnityEngine.Random.Range(0, availableSpawns.Length)];
            occupiedSpawnPoints.Add(randomSpawn);
            return randomSpawn;
        }
    }
}
