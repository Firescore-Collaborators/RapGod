using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(RewardPhasesControl))]
    public class TipJarState : State
    {
        private void Awake()
        {
            mRewardPhasesControl = GetComponent<RewardPhasesControl>();

            mTipJarUi.SetActive(false);
        }

        public override void OnEnter()
        {
            //if (ProgressUtils.IsLastLevelInMilestone(Progress.Instance.CurrentLevel))
            //{
            //    mTipJarUi.SetActive(true);
            //    Progress.Instance.Currency += 150; //< TODO: update
            //}
            //else
            //{
                mRewardPhasesControl._OnPhaseComplete();
            //}
        }

        public override void OnExit()
        {
            mTipJarUi.SetActive(false);
        }

        [SerializeField]
        private GameObject mTipJarUi;

        private RewardPhasesControl mRewardPhasesControl;
    }
}
