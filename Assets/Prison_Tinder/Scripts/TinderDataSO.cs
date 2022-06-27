using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TinderData", menuName = "Tinder/TinderData", order = 1)]
public class TinderDataSO : ScriptableObject
{
    public string userId;
    public Texture2D userDP;
    public string recipientId;
    public Texture2D recipientDP;
}
