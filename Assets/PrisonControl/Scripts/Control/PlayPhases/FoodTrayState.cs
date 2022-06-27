using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class FoodTrayState : State
    {
        public override void OnEnter()
        {
            ui.SetActive(true);
            step.SetActive(true);
            cameraManager.ActivateDefaultCam();
            RenderSettings.fogDensity = 0.08f;
        }

        public override void OnExit()
        {
            ui.SetActive(false);
            step.SetActive(false);
            RenderSettings.fogDensity = 0;

            Progress.Instance.IsLunchBoxStepTutDone = true;
        }

        [SerializeField]
        private GameObject ui, step;

        [SerializeField]
        private CameraManager cameraManager;
    }
}
