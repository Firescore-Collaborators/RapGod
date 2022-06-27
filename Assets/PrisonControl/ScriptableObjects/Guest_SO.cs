using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Guest", menuName = "PrisonControl/Guest", order = 55)]
public class Guest_SO : ScriptableObject
{
    public GameObject pf_character;
    public int queueNo;
    public Sprite DP;

}
