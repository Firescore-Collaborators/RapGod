using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(RewardPhasesControl))]
    public class CelebrationState : State
    {
        private void Awake()
        {
            mRewardPhasesControl = GetComponent<RewardPhasesControl>();

            mCelebrationUi.SetActive(false);
        }

        public override void OnEnter()
        {
            mCelebrationUi.SetActive(true);
        }

        public override void OnExit()
        {
            mCelebrationUi.SetActive(false);
        }

        [SerializeField]
        private GameObject mCelebrationUi;

        private RewardPhasesControl mRewardPhasesControl;
    }
}
