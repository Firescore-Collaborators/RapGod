using UnityEngine;

namespace PrisonControl
{
    [CreateAssetMenu(fileName = "CellCheck", menuName = "PrisonControl/CellCheck", order = 51)]
    public class CellCheck_SO : ScriptableObject
    {
        public GameObject pf_cell;
        public GameObject pf_hiddenObject;
        public GameObject prisoner;
        public string prisonerDialogue;
        public string prisonerExitDialogue;

        public AudioClip audDialogue;
        public AudioClip audDialogueExit;

        public Punishment punishment;
    }
}
