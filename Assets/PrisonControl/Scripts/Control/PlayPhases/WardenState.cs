using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class WardenState : State
    {
        private void Awake()
        {
            mPlayPhasesControl = GetComponent<PlayPhasesControl>();
        }

        public override void OnEnter()
        {
            Room.SetActive(true);
            ui.SetActive(true);
            step.SetActive(true);

            if (!Progress.Instance.IsAudioExperianceDone)
            {
                coinsPanel.SetActive(false);
                audioExperiancePanel.SetActive(true);
            }

        }

        public override void OnExit()
        {
      //      Room.SetActive(false);
            ui.SetActive(false);
            step.SetActive(false);

            if (!Progress.Instance.IsAudioExperianceDone)
            {
                coinsPanel.SetActive(true);
                audioExperiancePanel.SetActive(false);
            }

            Progress.Instance.IsAudioExperianceDone = true;
        }

        private PlayPhasesControl mPlayPhasesControl;

        [SerializeField]
        private GameObject ui, step, audioExperiancePanel, coinsPanel;

        [SerializeField]
        private CameraManager cameraManager;

        [SerializeField]
        private GameObject Room;
    }
}

