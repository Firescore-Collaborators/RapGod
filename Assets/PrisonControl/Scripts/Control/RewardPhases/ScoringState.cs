using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(RewardPhasesControl))]
    public class ScoringState : State
    {
        private void Awake()
        {
            mRewardPhasesControl = GetComponent<RewardPhasesControl>();

            mScoringUi.SetActive(false);
        }

        public override void OnEnter()
        {
            mScoringUi.SetActive(true);
        }

        public override void OnExit()
        {
            Invoke("Delay", 0.5f);
        }

        void Delay()
        {
            mScoringUi.SetActive(false);
        }

        [SerializeField]
        private GameObject mScoringUi;

        private RewardPhasesControl mRewardPhasesControl;
    }
}
