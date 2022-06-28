using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterListSO", menuName = "RapBattle/CharacterListSO")]
public class CharacterListSO : ScriptableObject
{
    public List<GameObject> femaleCharacterList;
    public List<GameObject> maleCharacterList;
}
