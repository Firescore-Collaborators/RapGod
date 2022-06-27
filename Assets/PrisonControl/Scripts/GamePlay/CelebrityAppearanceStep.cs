using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

namespace PrisonControl
{
    public class CelebrityAppearanceStep : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        [SerializeField]
        private GamePlayStep _mGamePlayStep;

        [SerializeField]
        private CopManager copManager;

        [Foldout("TextMeshPro")]
        public TextMeshPro popupText;

        [SerializeField]
        private GameObject popUp, checkPanel;

        void OnEnable()
        {
            AssignData();
            StartCoroutine(Steps());
        }

        IEnumerator Steps()
        {
            yield return new WaitForSeconds(.5f);
            popUp.SetActive(true);
           
            yield return new WaitForSeconds(1);
            checkPanel.SetActive(true);
        }

        void AssignData()
        {
        //    popupText.text = "" + _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetCelebrityAppearanceInfo[_mPlayPhasesControl.guestAppearance_cursr].popUpText;
        }

        public void Yes()
        {
            copManager.Catch();
            
            Timer.Delay(2, () =>
            {
                _mPlayPhasesControl._OnMiniLevelFinished2();
            });
            HideId();
        }

        public void No()
        {
            _mPlayPhasesControl._OnMiniLevelFinished();
            HideId();
        }

        void HideId()
        {
            popUp.SetActive(false);
            checkPanel.SetActive(false);
        }
    }
}
