using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class IDCheckState : State
    {
        public override void OnEnter()
        {
            ui.SetActive(true);
            step.SetActive(true);
            cameraManager.ActivateDefaultCam();

        }

        public override void OnExit()
        {
            ui.SetActive(false);
            step.SetActive(false);

         //   RenderSettings.fogDensity = 0;
        }

        [SerializeField]
        private GameObject ui, step;

        [SerializeField]
        private CameraManager cameraManager;
    }
}
