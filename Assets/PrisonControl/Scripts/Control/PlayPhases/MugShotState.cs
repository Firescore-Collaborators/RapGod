using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class MugShotState : State
    {
        public override void OnEnter()
        {
            Room.SetActive(true);
            ui.SetActive(true);
            step.SetActive(true);

            cameraManager.ActivateCamMugshot();
        }

        public override void OnExit()
        {
      //      Room.SetActive(false);
            ui.SetActive(false);
            step.SetActive(false);
        }

        [SerializeField]
        private GameObject ui, step;

        [SerializeField]
        private CameraManager cameraManager;

        [SerializeField]
        private GameObject Room;
    }
}



