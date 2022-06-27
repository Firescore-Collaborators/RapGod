using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "VisitorInmate", menuName = "PhoneBooth/VisitorInmate")]
public class VisitorInmatePair : ScriptableObject
{
    public GameObject visitor;
    public GameObject inmate;
}
