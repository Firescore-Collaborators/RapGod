using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    enum RewardPhase
    {
      // Celebration,
        Scoring,
        ItemUnlock,
        TipJar,
        Transition
    }

    public class RewardPhasesControl : State
    {
        public event System.Action OnRewardsComplete;

        private void Awake()
        {
            mRewardPhasesStateMachine = new StateMachine<RewardPhase>(
           //     (RewardPhase.Celebration, GetComponent<CelebrationState>()),
                (RewardPhase.Scoring, GetComponent<ScoringState>()),
                (RewardPhase.ItemUnlock, GetComponent<ItemUnlockState>()),
                (RewardPhase.TipJar, GetComponent<TipJarState>()),
                (RewardPhase.Transition, GetComponent<TransitionState>())
            );

            mRewardUi.SetActive(false);
        }

        public override void OnEnter()
        {
            mRewardUi.SetActive(true);
            mPhaseCursor = 0;
            BeginNextPhase();
        }

        public override void OnExit()
        {
        }

        private void BeginNextPhase()
        {
            mRewardPhasesStateMachine.SwitchState(PHASES[mPhaseCursor]);
        }

        // To be called by the states
        public void _OnPhaseComplete()
        {
            ++mPhaseCursor;
            if (mPhaseCursor == PHASES.Length)
            {
                mRewardUi.SetActive(false);
                mRewardPhasesStateMachine.Reset();
                OnRewardsComplete?.Invoke();
            }
            else
            {
                BeginNextPhase();
            }
        }

        [SerializeField]
        private GameObject mRewardUi;

        private int mPhaseCursor;
        private StateMachine<RewardPhase> mRewardPhasesStateMachine;

        private static readonly RewardPhase[] PHASES =
        {
           // RewardPhase.Celebration,
            RewardPhase.Scoring,
            RewardPhase.ItemUnlock,
            RewardPhase.TipJar,
            RewardPhase.Transition,
        };
    }
}
