using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ID Check", menuName = "PrisonControl/ID Check", order = 51)]
public class IDCheck_SO : ScriptableObject
{
    public string popUpText;

    public string relation;

    public string ID_name;
    public int ID_age;

    public WrongInfo wrongInfo;

    public AudioClip audio_intro;

    public enum WrongInfo
    {
        photo,
        name,
        relation,
        none
    }
}
