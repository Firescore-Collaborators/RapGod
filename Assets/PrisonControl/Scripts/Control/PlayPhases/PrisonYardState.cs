using UnityEngine;
using System.Collections;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class PrisonYardState : State
    {
        public override void OnEnter()
        {
            Room.SetActive(true);
            ui.SetActive(true);
            step.SetActive(true);

            cameraManager.ActivateCamPrisonYardEnter();

            StartCoroutine(Delay());
        }

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(.2f);
            cameraManager.ActivateCamPrisonYard(2);
        }

        public override void OnExit()
        {
      //      Room.SetActive(false);
            ui.SetActive(false);
            step.SetActive(false);
            RenderSettings.fogDensity = 0.08f;
        }

        [SerializeField]
        private GameObject ui, step;

        [SerializeField]
        private CameraManager cameraManager;

        [SerializeField]
        private GameObject Room;
    }
}




