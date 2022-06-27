using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace PrisonControl
{
    public class PrisonYardUi : MonoBehaviour
    {
        [SerializeField]
        private GameObject popUp, panel_response;

        [SerializeField]
        private Text txt_popup;

        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        public Action<bool> _OnActionTaken;
        public Action _OnLevelEnd;


        [SerializeField]
        private Image progressBar;

        public float totalScenarios;

        public float scenarioDone;

        private void OnEnable()
        {
            scenarioDone = 0;
            progressBar.fillAmount = 0;
            totalScenarios = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetPrisonYardInfo.scenarioInfo.Length;
        }

        public void ShowPopUp(int index)
        {
            popUp.SetActive(true);
            panel_response.SetActive(true);

            txt_popup.text = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetPrisonYardInfo.scenarioInfo[index];
        }

        public void HidePopup()
        {
            popUp.SetActive(false);
        }

        public void TakeAction(bool isPositive)
        {
            _OnActionTaken?.Invoke(isPositive);
            panel_response.SetActive(false);

            if (!isPositive)
            {
                // Punishment
                popUp.SetActive(false);
            }
            else
            {
             //   MoveToNextScenario();
            }
        }

        public void MoveToNextScenario()
        {

            scenarioDone++;
            progressBar.fillAmount = scenarioDone / totalScenarios;

            HidePopup();

            Timer.Delay(2f, () =>
            {
                if (scenarioDone >= totalScenarios)
                {
                    _OnLevelEnd?.Invoke();
                }
            });
        }
    }
}
