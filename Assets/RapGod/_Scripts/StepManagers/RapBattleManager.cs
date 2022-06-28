using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapBattleManager : MonoBehaviour
{
    public CharacterListSO characterList;
    public Transform spawnPos;

    void OnEnable()
    {
        Init();
    }

    void OnDisable()
    {

    }

    void Init()
    {
        SpawnCharacters();
    }

    void SpawnCharacters()
    {
        characterList.SpawnCharacter(spawnPos);
    }
}

