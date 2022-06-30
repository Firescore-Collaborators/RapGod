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
            //if (ProgressUtils.IsLevelEnd(Progress.Instance.CurrentLevel - 1) && !Progress.Instance.IsTimedEventScheduled)
            if(ProgressUtils.IsLevelEnd(Progress.Instance.CurrentLevel - 1))
            {
                mScoringUi.SetActive(true);
                Debug.Log("level end playing");
            }
            else
            {
                //Debug.Log("level end skipping " + Progress.Instance.IsTimedEventScheduled);
                Debug.Log("level end skipping " + ProgressUtils.IsLevelEnd(Progress.Instance.CurrentLevel - 1));
                mRewardPhasesControl._OnPhaseComplete();
            }
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
