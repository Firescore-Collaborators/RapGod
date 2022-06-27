using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1LoadData : LevelObject
{
    public GameObject rapper1;
    public GameObject rapper2;

    public Transform rapper1Pos;
    public Transform rapper2Pos;


    void OnEnable()
    {
        InstantiateCharacter(rapper1,rapper1Pos);
        InstantiateCharacter(rapper2,rapper2Pos);
        
    }

    void InstantiateCharacter(GameObject character,Transform position)
    {
        GameObject spawnedChar = Instantiate(character);
        spawnedChar.transform.parent = position;
        spawnedChar.transform.localPosition = Vector3.zero;
        spawnedChar.transform.localRotation = Quaternion.Euler(Vector3.zero);
        spawnedChar.transform.localScale = Vector3.one;
    }

}

