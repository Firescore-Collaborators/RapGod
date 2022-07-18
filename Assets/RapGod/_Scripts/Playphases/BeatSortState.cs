using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PlayPhasesControl))]
    public class BeatSortState : State
    {
        public override void OnEnter()
        {
            ui.SetActive(true);
            step.SetActive(true);
        }
        public override void OnExit()
        {
            ui.SetActive(false);
            step.SetActive(false);
        }

        [SerializeField]
        private GameObject ui, step;

    }
}
