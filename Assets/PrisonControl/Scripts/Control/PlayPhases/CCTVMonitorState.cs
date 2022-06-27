using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class CCTVMonitorState : State
    {
        public override void OnEnter()
        {
            Room.SetActive(true);
            ui.SetActive(true);
            step.SetActive(true);
        }

        public override void OnExit()
        {
     //       Room.SetActive(false);
            ui.SetActive(false);
            step.SetActive(false);

            cameraManager.cinemachineBrainCCTV.enabled = false;
            cameraManager.cinemachineBrainMain.enabled = true;
        }

        [SerializeField]
        private GameObject ui, step;

        [SerializeField]
        private GameObject Room;


        [SerializeField]
        private CameraManager cameraManager;
    }
}





