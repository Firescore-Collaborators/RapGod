using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class PhoneBoothState : State
    {
        public override void OnEnter()
        {
            ui.SetActive(true);
            step.SetActive(true);
            camOld.SetActive(false);
            camNew.SetActive(true);
            Room.SetActive(true);

        }

        public override void OnExit()
        {
            ui.SetActive(false);
            step.SetActive(false);
            camNew.GetComponent<CameraController>().SetCurrentCamera(Cameras.defaultCam);
            camOld.GetComponent<CameraManager>().ActivateDefaultCam();
            camNew.SetActive(false);
            camOld.SetActive(true);
       //     Room.SetActive(false);
        }

        [SerializeField]
        private GameObject ui, step,camOld,camNew;

        [SerializeField]
        private GameObject Room;
    }
}
