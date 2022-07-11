using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentSO", menuName = "RapBattle/AgentUI/AgentSO")]
public class AgentsList_SO : ScriptableObject
{
    public List<Agent_SO> agentList = new List<Agent_SO>();
}
