using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DancerData
{
    public Dancers_SO dancerSO;
    public bool rightOption;
}

[System.Serializable]
public class Requirement
{
    public string requirementText;
    public bool allowed;
}
[System.Serializable]
public enum GirlAuditionAnimation
{
    Twerk,
}

[CreateAssetMenu(fileName = "DancersListSO", menuName = "RapGod/Dancers/DancersListSO", order = 51)]
public class DancerSOList : ScriptableObject
{
    public List<DancerData> dancersList = new List<DancerData>();
    public List<Requirement> requirements = new List<Requirement>();
    public GirlAuditionAnimation girlanim;
    public InputSequenceSO inputSequenceSO;
    public float tapSmashLimit;

}
