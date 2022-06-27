using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrisonControl
{
    public class HubUi : MonoBehaviour
    {
        // TODO: remove
        public void SimulateProgress()
        {
            ++Progress.Instance.CurrentLevel;
        }

        private void Awake()
        {
            UpdateDayText();
            UpdateUnlockProgress();
            UpdateCurrentLevelText();

            Progress.Instance.OnCurrentLevelChanged += OnCurrentLevelChanged;
            Progress.Instance.OnCurrentMilestoneChanged += UpdateDayText;
        }

        private void OnDestroy()
        {
            Progress.Instance.OnCurrentLevelChanged -= OnCurrentLevelChanged;
            Progress.Instance.OnCurrentMilestoneChanged -= UpdateDayText;
        }

        private void OnCurrentLevelChanged()
        {
            UpdateCurrentLevelText();
            UpdateUnlockProgress();
        }

        private void UpdateDayText()
        {
            mDayText.text = $"Day {Progress.Instance.CurrentMilestone}";
        }

        private void UpdateUnlockProgress()
        {
            string itemName = "";
            UnlockInfo info = Progress.Instance.GetNextUnlockInfo(Progress.Instance.CurrentLevel);
            if (info.nextItem.IsPunishment())
                itemName = info.nextItem.punishment.ToString();
            else if (info.nextItem.IsPowder())
                itemName = info.nextItem.Powder.ToString();

         //   mUnlockProgress.text = $"{itemName}\n{(int)(info.unlockProgressRatio * 100f)}%";
        }

        private void UpdateCurrentLevelText()
        {
            int milestone = ProgressUtils.GetMilestoneFromLevel(Progress.Instance.CurrentLevel);
            int firstLevelInMilestone = ProgressUtils.GetFirstLevelOfMilestone(milestone);
            int firstLevelInNextMilestone = ProgressUtils.GetFirstLevelOfMilestone(milestone + 1);

            int levelNumberInMilestone = Progress.Instance.CurrentLevel - firstLevelInMilestone + 1;
            int levelCountInMilestone = firstLevelInNextMilestone - firstLevelInMilestone;
            mCurrentLevelText.text = $"Level {levelNumberInMilestone} of {levelCountInMilestone}";

            // Remove Number of hands on Tap To Start
            for (int i = 0; i < contentDay.transform.childCount; i++)
            {
                Destroy(contentDay.transform.GetChild(i).gameObject);
            }

            for (int i = 1; i <= levelCountInMilestone; i ++)
            {
                GameObject obj = Instantiate(pf_day, contentDay.transform);

                if ( i < levelNumberInMilestone)
                {
                    obj.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }

        [SerializeField]
        private Text mDayText;
       
        [SerializeField]
        private Text mCurrentLevelText;

        [SerializeField]
        private GameObject pf_day, contentDay;

        [SerializeField]
        PlayPhasesControl _mPlayPhasesControl;
    }
}
