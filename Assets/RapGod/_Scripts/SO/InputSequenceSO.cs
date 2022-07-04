using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputSequenceSO", menuName = "RapBattle/MultipInput/InputSequenceSO")]
public class InputSequenceSO : ScriptableObject
{
    public List<TouchInputType> inputSequence;
}
