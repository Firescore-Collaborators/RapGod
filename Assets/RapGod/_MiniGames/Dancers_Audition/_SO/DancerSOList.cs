using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DancersListSO", menuName = "RapGod/Dancers/DancersListSO", order = 51)]
public class DancerSOList : ScriptableObject
{
    public List<Dancers_SO> dancersList = new List<Dancers_SO>();
}
