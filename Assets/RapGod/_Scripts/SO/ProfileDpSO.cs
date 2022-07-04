using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FollowerCount
{
    public int followerPositive;
    public int followerNegative;
}

[CreateAssetMenu(fileName = "New ProfileDpSO", menuName = "RapBattle/ProfileDpSo/ProfileDpSO")]
public class ProfileDpSO : ScriptableObject
{
    public FollowerCount followerCount;
    public List<Texture2D> profileDpList;
    public string option1;
    public string option2;
}
