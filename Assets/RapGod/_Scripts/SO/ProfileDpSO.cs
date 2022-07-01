using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ProfileDpSO", menuName = "RapBattle/ProfileDpSo/ProfileDpSO")]
public class ProfileDpSO : ScriptableObject
{
    public List<Texture2D> profileDpList;
    public string option1;
    public string option2;
}
