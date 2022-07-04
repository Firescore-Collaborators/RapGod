using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum HandShakeType
{
    Talking, 
    Annoyed,
}

[CreateAssetMenu(fileName = "HandShakeSO", menuName = "RapBattle/HandShake/HandShakeSO")]
public class HandShakeSO : ScriptableObject
{
    public List<HandShakeType> player = new List<HandShakeType>();
    public List<HandShakeType> enemy = new List<HandShakeType>();
    public InputSequenceSO inputSequence;
}
