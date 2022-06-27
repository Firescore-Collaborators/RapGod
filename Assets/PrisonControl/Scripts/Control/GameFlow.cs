using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tabtale.TTPlugins;

namespace PrisonControl
{
    public enum GameState
    {
        Hub,
        Play,
        Items,
        Tools,
        Salon,
        Avatar
    }

    public class GameFlow : MonoBehaviour
    {
        private void Awake()
        {
            TTPCore.Setup();

            mStateMachine = new StateMachine<GameState>(
                (GameState.Play, GetComponent<GameplayState>()),
                (GameState.Hub, GetComponent<HubState>()),
                (GameState.Items, GetComponent<ItemsState>()),
                (GameState.Salon, GetComponent<JailState>()),
                (GameState.Avatar, GetComponent<AvatarSelectionState>())
            );


            Debug.Log("hh 1"+(transform.forward));
            Debug.Log("hh 1" + (transform.right));

            Debug.Log("hh 1" + (transform.forward - transform.right).normalized);

        }

        private void Start()
        {
            if (Progress.Instance.CurrentLevel == 1) {

                //if (!Progress.Instance.IsAvatarSelected)
                //    mStateMachine.SwitchState(GameState.Avatar);
                //else
                    mStateMachine.SwitchState(GameState.Play);
            }
            else
                mStateMachine.SwitchState(GameState.Hub);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
                mStateMachine.GetCurrentState()?.OnBackButtonPressed();
        }

        // Only meant to be called by GameFlow's states
        public void _SwitchState(GameState gameState)
        {
            mStateMachine.SwitchState(gameState);
        }

        private StateMachine<GameState> mStateMachine;

        public GameObject fadeIn;
    }
}
