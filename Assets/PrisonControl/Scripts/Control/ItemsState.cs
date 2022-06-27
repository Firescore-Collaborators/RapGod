using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(GameFlow))]
    public class ItemsState : State
    {
        private void Awake()
        {
            mGameFlow = GetComponent<GameFlow>();
        }

        public override void OnEnter()
        {
            PopupManager.Instance.PushPopup(PopupId.Items);
        }

        public override void OnExit()
        {
            PopupManager.Instance.PopPopup();
        }

        public override bool OnBackButtonPressed()
        {
            mGameFlow._SwitchState(GameState.Hub);
            return true;
        }

        private GameFlow mGameFlow;
    }
}
