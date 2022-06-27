using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrisonControl
{
    public class PlayPhasesUi : MonoBehaviour
    {
        public void SetProgress(PlayPhase currentPhase, int currentPhaseNumber, int totalPhaseCount)
        {
            mText.text = $"{currentPhase.ToString()} : {currentPhaseNumber} of {totalPhaseCount}";
        }

        [SerializeField]
        private Text mText;
    }
}
