using UnityEngine;


namespace PrisonControl
{
    [CreateAssetMenu(fileName = "Warden", menuName = "PrisonControl/Warden", order = 51)]
    public class Warden_SO : ScriptableObject
    {
        public WardenGameType wardenGameType;

        public AudioClip[] aud_negetiveConv;
        public AudioClip[] aud_positiveConv;
        public AudioClip[] aud_positiveResponse;
        public AudioClip[] aud_negetiveResponse;

        public string[] negetive_conversation;
        public string[] conversation;

        public string[] positiveResponse;
        public string[] negetiveResponse;

        public GameObject tool;

        public bool isToolUnlock;

        public Punishment punishmentType;

        public AnimationType[] positive_anim;
        public AnimationType[] negetive_anim;

        public PrisonerAnimationTypes[] prisoner_anim;

        public bool isFemalePrisoner;

        public WardenPos wardenPos;

        public enum AnimationType
        {
            Talking1,
            YellingOut,
            StandingGreeting,
            Salute,
            HappyRightTurn,
            Excited,
            Defeated,
            Annoyed,
            SittingIdle,
            SittingTalk1,
            SittingTalk2,
            SittingUpset1,
            SittingUpset2,
        }

        public enum WardenGameType {
            Normal,
            Mugshot,
            TinderBio,
            TinderChat
        }

        public enum WardenPos
        {
            standing1,
            standing2,
            sittingChair,
            sittingSofa
        }

    }
}
