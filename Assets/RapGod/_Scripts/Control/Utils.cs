using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrisonControl;
public static class Utils
{
    public static GameObject SpawnChar(GameObject character, int type, Transform pos)
    {

        GameObject spawnedCharacter = GameObject.Instantiate(character);
        spawnedCharacter.transform.GetChild(type).gameObject.SetActive(true);
        spawnedCharacter.transform.parent = pos;
        spawnedCharacter.transform.Reset();
        return spawnedCharacter;
    }

    public static GameObject spawnGameObject(GameObject gameObject, Transform pos)
    {
        GameObject spawnedObject = GameObject.Instantiate(gameObject);
        spawnedObject.transform.parent = pos;
        spawnedObject.transform.Reset();
        return spawnedObject;
    }

    public static GameObject SpawnCharacter(this CharacterListSO charList, Transform pos)
    {
        List<GameObject> selectedCharList = new List<GameObject>();
        if (Progress.Instance.AvatarGender == 0)
        {
            selectedCharList = charList.femaleCharacterList;
        }
        else
        {
            selectedCharList = charList.maleCharacterList;
        }
        return Utils.SpawnChar(selectedCharList[0], Progress.Instance.AvatarType, pos);
    }

    public static Sprite NewSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
