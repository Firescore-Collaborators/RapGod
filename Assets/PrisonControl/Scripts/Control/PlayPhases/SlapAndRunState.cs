using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class SlapAndRunState : State
    {
        public override void OnEnter()
        {
            Room.SetActive(true);
            ui.SetActive(true);
            step.SetActive(true);

            RenderSettings.fogDensity = 0f;

            audioManager.SetSlapAndRunBg();
        }

        public override void OnExit()
        {
       //     Room.SetActive(false);
            ui.SetActive(false);
            step.SetActive(false);

            RenderSettings.fogDensity = 0.08f;
        }

        [SerializeField]
        private GameObject ui, step;

        [SerializeField]
        private GameObject Room;

        [SerializeField]
        private AudioManager audioManager;

    }
}







