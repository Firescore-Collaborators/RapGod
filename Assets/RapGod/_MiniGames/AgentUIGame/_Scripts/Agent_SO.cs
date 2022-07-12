using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AgentUI", menuName = "RapBattle/AgentUI", order = 51)]
public class Agent_SO : ScriptableObject
{
    public GameObject Model;
    public Sprite AgentPic;
    public string AgentName;
    public float CashMultiplier, FollowerMultiplier;
}
