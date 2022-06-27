using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class JailEntryState : State
    {
        private void Awake()
        {
            mPlayPhasesControl = GetComponent<PlayPhasesControl>();
        }

        public override void OnEnter()
        {
            set.SetActive(true);
            ui.SetActive(true);
            step.SetActive(true);

            cameraManager.ActivateCamJailEntry1();

            RenderSettings.fogDensity = 0.04f; 
        }

        public override void OnExit()
        {
            set.SetActive(false);
            ui.SetActive(false);
            step.SetActive(false);

            RenderSettings.fogDensity = 0;

        }

        private PlayPhasesControl mPlayPhasesControl;

        [SerializeField]
        private GameObject ui, step, set;

        [SerializeField]
        private CameraManager cameraManager;
  
    }
}


