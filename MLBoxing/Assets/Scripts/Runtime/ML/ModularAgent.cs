using MLBoxing.Ragdoll;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine;

namespace MLBoxing.ML {
    public class ModularAgent : Agent {
        public Action<ModularAgent> onInitialize;
        public Action<ModularAgent> onTerminated;
        public Action<ModularAgent> onFixedUpdate;
        public Action<ModularAgent> onOpponentChanged;
        public Action<ModularAgent> onWin;
        public Action<ModularAgent> onLose;
        public Action<ModularAgent> onDefeated;
        public Action<ModularAgent, float> onTakeDamage;
        public Action<ModularAgent, float> onDealDamage;

        [SerializeField]
        public RagdollModel ragdoll;
        [SerializeField]
        public ModularAgent opponent;
        [SerializeField]
        BehaviorParameters behaviorParameters = default;

        public float score { get; private set; }
        public int team { get => behaviorParameters.TeamId; set => behaviorParameters.TeamId = value; }

        bool dead = false;


        Dictionary<string, float> rewardSources = new Dictionary<string, float>();
        Dictionary<string, float> scoreSources = new Dictionary<string, float>();

        public void SetOpponent(ModularAgent agent) {
            opponent = agent;
            onOpponentChanged?.Invoke(this);
        }

        public override void Initialize() {
            onInitialize?.Invoke(this);
        }

        public override void OnEpisodeBegin() {
            foreach(var pair in rewardSources) {
                Academy.Instance.StatsRecorder.Add("Reward/"+pair.Key, pair.Value);
            }
            foreach (var pair in scoreSources) {
                Academy.Instance.StatsRecorder.Add("Score/"+pair.Key, pair.Value);
            }
            rewardSources.Clear();
            scoreSources.Clear();
            if (dead) {
                Destroy(gameObject);
            }
        }


        public void AddReward(float reward, string source, bool asScore = false) {
            if (asScore) {
                if (scoreSources.ContainsKey(source)) {
                    scoreSources[source] += reward;
                } else {
                    scoreSources[source] = reward;
                }
                AddScore(reward);
            } else {
                if (rewardSources.ContainsKey(source)) {
                    rewardSources[source] += reward;
                } else {
                    rewardSources[source] = reward;
                }
                AddReward(reward);
            }
        }

        public void AddScore(float summand) {
            score += summand;
        }

        public void Win() {
            onWin?.Invoke(this);
        }

        public void Lose() {
            onLose?.Invoke(this);
        }

        /// <summary>
        /// Kills the agent
        /// </summary>
        public void Kill() {
            if (dead) {
                return;
            }
            dead = true;
            EndEpisode();
            score = 0;
        }

        /// <summary>
        /// Terminte calls onTerminate and Kills the agent after
        /// </summary
        public void Terminate() {
            onTerminated?.Invoke(this);
            Kill();
        }

        void DealDamage(float damage) {
            onDealDamage?.Invoke(this, damage);
        }

        void TakeDamage(float damage) {
            onTakeDamage?.Invoke(this, damage);
        }

        private void Awake() {
            foreach(var hitbox in ragdoll.allHitboxes) {
                hitbox.onHit += DealDamage;
            }
            foreach(var hurtbox in ragdoll.allHurtboxes) {
                hurtbox.onHurt += TakeDamage;
            }
        }

        void FixedUpdate() {
            onFixedUpdate?.Invoke(this);
        }
    }
}
