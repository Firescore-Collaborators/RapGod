using UnityEngine;

namespace PrisonControl
{
    [CreateAssetMenu(fileName = "Interrogation", menuName = "PrisonControl/Interrogation", order = 51)]
    public class Interrogation_SO : ScriptableObject
    {
        public string intro;
        public string reasonText;

        public string[] conversation;

        public string[] positiveResponse;
        public string[] negetiveResponse;


        public AudioClip aud_intro, aud_reason;
        public AudioClip[] aud_conversation;
        public AudioClip[] aud_positiveResponse;
        public AudioClip[] aud_negetiveResponse;

        public bool isGuilty;

        public Punishment punishmentType;
    }
}
