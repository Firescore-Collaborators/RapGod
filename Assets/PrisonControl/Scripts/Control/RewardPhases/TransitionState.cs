using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(RewardPhasesControl))]
    public class TransitionState : State
    {
        private void Awake()
        {
            mRewardPhasesControl = GetComponent<RewardPhasesControl>();
        }

        public override void OnEnter()
        {
            transitionPanel.DoTransition(() =>
            {
                mRewardPhasesControl._OnPhaseComplete();
            }
            );
        }

        public override void OnExit()
        {

        }

        private RewardPhasesControl mRewardPhasesControl;

        [SerializeField]
        private TransitionPanel transitionPanel;

    }
}
