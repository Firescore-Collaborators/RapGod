using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class CellCheckState : State
    {
        public override void OnEnter()
        {
            Room.SetActive(true);
            ui.SetActive(true);
            step.SetActive(true);


        }

        public override void OnExit()
        {
     //       Room.SetActive(false);
            ui.SetActive(false);
            step.SetActive(false);
        }

        [SerializeField]
        private GameObject ui, step;

        [SerializeField]
        private GameObject Room;
    }
}






