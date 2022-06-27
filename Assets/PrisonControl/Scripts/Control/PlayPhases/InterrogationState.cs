using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class InterrogationState : State
    {
        private void Awake()
        {
            mPlayPhasesControl = GetComponent<PlayPhasesControl>();
        }

        public override void OnEnter()
        {
            ui.SetActive(true);
            step.SetActive(true);

            cameraManager.ActivateCamInterrogation();
            audioManager.SetInterrogationBg();
        }

        public override void OnExit()
        {
            ui.SetActive(false);
            step.SetActive(false);
        }

        private PlayPhasesControl mPlayPhasesControl;

        [SerializeField]
        private GameObject ui, step;

        [SerializeField]
        private CameraManager cameraManager;

        [SerializeField]
        private AudioManager audioManager;

    }
}
