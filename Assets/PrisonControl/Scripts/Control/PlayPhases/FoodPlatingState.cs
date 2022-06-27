using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class FoodPlatingState : State
    {
        private void Awake()
        {
            mPlayPhasesControl = GetComponent<PlayPhasesControl>();
        }

        public override void OnEnter()
        {
            ui.SetActive(true);
            step.SetActive(true);
            set.SetActive(true);

            cameraManager.ActivateCamFoodPlating1(0f);
        }

        public override void OnExit()
        {
            ui.SetActive(false);
            step.SetActive(false);
            //       set.SetActive(false);

            Progress.Instance.IsfoodPlatingTutDone = true;

        }

        private PlayPhasesControl mPlayPhasesControl;

        [SerializeField]
        private GameObject ui, step, set;

        [SerializeField]
        private CameraManager cameraManager;
    }
}

