using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NarrationAnimation
{
    Talking1,
    Annoyed
}

[System.Serializable]
public class TopChart
{
    public bool toShow;
    public int showAtIndex;
}

[CreateAssetMenu(fileName = "Narration", menuName = "RapBattle/Narration/NarrationSO")]
public class NarrationSO : ScriptableObject
{
    public TopChart topChart;
    public AudioClip[] aud_negetiveConv;
    public AudioClip[] aud_defaultConv;
    public AudioClip[] aud_positiveConv;
    public AudioClip[] aud_positiveResponse;
    public AudioClip[] aud_negetiveResponse;

    public string[] negetive_conversation;
    public string[] positive_conversation;
    public string[] default_conversation;

    public string[] positiveResponse;
    public string[] negativeResponse;

    public NarrationAnimation[] positive_anim;
    public NarrationAnimation[] default_anim;
    public NarrationAnimation[] negetive_anim;

}
