using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dancers", menuName = "RapGod/Dancers", order = 51)]
public class Dancers_SO : ScriptableObject
{
    public string[] Conversation;
    public string[] responsePositive;
    public string[] responseNegetive;
    public string[] Resume;

    public GameObject character;

}

