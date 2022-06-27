using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [CreateAssetMenu(fileName = "JailEntry", menuName = "PrisonControl/JailEntry", order = 51)]
    public class JailEntry_SO : ScriptableObject
    {
        public Prisoner_SO prisoner;

        public string firstDialogue;
        public AudioClip aud_firstDialogue;

        public List<GameObject> listHiddenObjects;
        public string caughtDialogue;
        public string copResponse;
        public AudioClip aud_caughtDialogue, aud_copResponse;

        public Punishment punishmentType1;

        public List<HidePos> hidePos;

        public Sprite scanImage;

        public bool isGuilty;

        public enum HidePos
        {
            HidePos1,
            HidePos2,
            HidePos3,
            HidePos4,
            HidePos5,
            HidePos6,
        }
    }
}
