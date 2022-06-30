using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NarrationAnimation
{
    Talking1,
    Annoyed
}

[CreateAssetMenu(fileName = "Narration", menuName = "RapBattle/Narration/NarrationSO")]
public class NarrationSO : ScriptableObject
{
    public AudioClip[] aud_negetiveConv;
        public AudioClip[] aud_positiveConv;
        public AudioClip[] aud_positiveResponse;
        public AudioClip[] aud_negetiveResponse;

        public string[] negetive_conversation;
        public string[] positive_conversation;
        public string[] default_conversation;

        public string[] positiveResponse;
        public string[] negetiveResponse;

        public NarrationAnimation[] positive_anim;
        public NarrationAnimation[] negetive_anim;

}
