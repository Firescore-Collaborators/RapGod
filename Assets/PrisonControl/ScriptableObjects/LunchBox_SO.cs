using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LunchBox", menuName = "PrisonControl/LunchBox", order = 51)]
public class LunchBox_SO : ScriptableObject
{
    [SerializeField]
    private GameObject lunchBox;

    [SerializeField]
    private InnocentTypes innocentType;

    [SerializeField]
    private string Message;

    public AudioClip aud_intro;

    public enum InnocentTypes
    {
        Innocent,
        SemiInnocent,
        Guilty
    }

    public GameObject ReturnLunchBoxPrefab()
    {
        return lunchBox;
    }

    public InnocentTypes ReturnInnocentType()
    {
        return innocentType;
    }

    public string ReturnMessage()
    {
        return Message;
    }

   
}
