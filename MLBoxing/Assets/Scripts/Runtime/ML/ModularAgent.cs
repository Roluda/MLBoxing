using MLBoxing.Character;
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

        public RagdollController controller => m_controller;
        public BoxingCharacter character => m_character;
        public ModularAgent opponent => m_opponent;

        public float score { get; private set; }


        [SerializeField]
        BehaviorParameters behaviorParameters = default;
        [SerializeField]
        RagdollController m_controller = default;
        [SerializeField]
        BoxingCharacter m_character = default;
        [SerializeField]
        ModularAgent m_opponent = default;

        bool dead = false;

        public void SetOpponent(ModularAgent agent) {
            m_opponent = agent;
            onOpponentChanged?.Invoke(this);
        }

        public override void Initialize() {
            onInitialize?.Invoke(this);
        }

        public override void OnEpisodeBegin() {
            if (dead) {
                Destroy(gameObject);
            }
        }

        public void SetTeam(int team) {
            behaviorParameters.TeamId = team;
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

        void FixedUpdate() {
            onFixedUpdate?.Invoke(this);
        }
    }
}
