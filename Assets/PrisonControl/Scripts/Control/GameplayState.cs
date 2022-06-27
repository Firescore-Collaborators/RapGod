using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tabtale.TTPlugins;

namespace PrisonControl
{
    enum PlayState
    {
        PlayPhases,
        RewardPhases
    }

    [RequireComponent(typeof(GameFlow))]
    public class GameplayState : State
    {
        private void Awake()
        {
            mGameFlow = GetComponent<GameFlow>();

            mStateMachine = new StateMachine<PlayState>(
                (PlayState.PlayPhases, mPlayPhasesControl),
                (PlayState.RewardPhases, mRewardPhasesControl)
            );

            mPlayPhasesControl.OnAllPhasesComplete += OnGameplayComplete;
            mPlayPhasesControl.OnGameplayCanceled += OnGameplayCanceled;
            mRewardPhasesControl.OnRewardsComplete += OnRewardsComplete;
        }

        private void OnDestroy()
        {
            mPlayPhasesControl.OnAllPhasesComplete -= OnGameplayComplete;
            mPlayPhasesControl.OnGameplayCanceled -= OnGameplayCanceled;
            mRewardPhasesControl.OnRewardsComplete -= OnRewardsComplete;
        }

        public override void OnEnter()
        {
            mStateMachine.SwitchState(PlayState.PlayPhases);
            lightingManager.ActivateGameLights();
        }

        public override void OnExit()
        {

        }

        public override bool OnBackButtonPressed()
        {
            State currentState = mStateMachine.GetCurrentState();
            if (currentState != null)
                return currentState.OnBackButtonPressed();
            else
                return false;
        }

        private void OnGameplayComplete()
        {
            LevelEnded();
            mStateMachine.SwitchState(PlayState.RewardPhases);
        }

        void LevelEnded()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("missionType", "levels");
            parameters.Add("missionID", Progress.Instance.CurrentLevel + (Progress.Instance.LevelMultiplier * mPlayPhasesControl.levels.Length));
            parameters.Add("missionStartedType", "new");
            TTPGameProgression.FirebaseEvents.MissionComplete(parameters);

            ++Progress.Instance.CurrentLevel;

            if (Progress.Instance.CurrentLevel > mPlayPhasesControl.levels.Length)
            {
                Progress.Instance.CurrentLevel = 1;
                Progress.Instance.LevelMultiplier++;
            }
        }

        private void OnGameplayCanceled()
        {
            mGameFlow._SwitchState(GameState.Hub);
        }

        private void OnRewardsComplete()
        {
            mGameFlow._SwitchState(GameState.Hub);
        }

        private GameFlow mGameFlow;
        private StateMachine<PlayState> mStateMachine;

        [SerializeField]
        private PlayPhasesControl mPlayPhasesControl;

        [SerializeField]
        private RewardPhasesControl mRewardPhasesControl;

        [SerializeField]
        private GameObject avatarLights, gameLights;

        [SerializeField]
        private LightingManager lightingManager;
    }
}
