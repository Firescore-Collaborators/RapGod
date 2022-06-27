using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class SlapAndRunStep : MonoBehaviour
    {
        [SerializeField]
        private PlayPhasesControl _mPlayPhasesControl;

        private SlapAndRun_SO slapAndRun_SO;

        [SerializeField]
        private Animator cellDoor;

        private SlapANdRun_Instantiate currentLevel;

        [SerializeField]
        private SlapAndRun_UiManager uiManager;

        [SerializeField]
        private CameraManager cameraManager;

        [SerializeField]
        private GameObject confitti;

        public GameObject tutorial;

        void OnEnable()
        {
            tutorial.SetActive(false);
            confitti.SetActive(false);
            cameraManager.SetBlendVal(0);
            slapAndRun_SO = _mPlayPhasesControl.levels[Progress.Instance.CurrentLevel - 1].GetSlapAndRunInfo;
            currentLevel = Instantiate(slapAndRun_SO.pf_level, transform).GetComponent<SlapANdRun_Instantiate>();

            currentLevel.GetComponent<SlapANdRun_Instantiate>().wardenString = slapAndRun_SO.wardenText;
         //   currentLevel.GetComponent<SlapANdRun_Instantiate>().temp_warden.GetComponent<SlapAndRun_warden>().ShowConversation(slapAndRun_SO.wardenText);
            currentLevel.uiManager = uiManager;
            currentLevel.GetComponent<SlapANdRun_Instantiate>().OnTutorialShow += OnGameStarted;
            currentLevel.GetComponent<SlapANdRun_Instantiate>().slapAndRunStep = this.gameObject.GetComponent<SlapAndRunStep>();
            cellDoor.enabled = false;
            currentLevel.slapAndRun_CellTrigger.anim = cellDoor;

            currentLevel.OnLevelEnd += OnLevelEnd;
            Invoke("ResetCameraBlend", 1f);
        }

        void OnGameStarted()
        {
            Debug.Log("its started");
            tutorial.SetActive(true);
        }
        void ResetCameraBlend()
        {
          
            cameraManager.SetBlendVal(1.5f);
        }

        private void OnDisable()
        {
           
            currentLevel.OnLevelEnd -= OnLevelEnd;
            currentLevel.GetComponent<SlapANdRun_Instantiate>().OnTutorialShow -= OnGameStarted;
        }

        public void OnTap()
        {
            currentLevel.GetComponent<SlapANdRun_Instantiate>().OnStartRun();
          
        }

        void OnLevelEnd()
        {
            confitti.SetActive(true);
            Timer.Delay(2, () =>
            {
                _mPlayPhasesControl._OnPhaseFinished();
                Destroy(currentLevel.gameObject);
            });

        }

        public void OnFailed()
        {
            for(int i = 0; i< gameObject.transform.childCount;i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
            _mPlayPhasesControl._OnRestart();
        }


    } 
        
    }
