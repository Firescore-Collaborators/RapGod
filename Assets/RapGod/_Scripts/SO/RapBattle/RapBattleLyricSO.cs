using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Levels
{
    public string RapString;
    public string OptionA;
    public string OptionB;
    public String MarkWord;
    public int CorrectAnswer;
}
[Serializable]
public class LevelData
{
    public List<Levels> leveldata;
}

[CreateAssetMenu(fileName = "RapBattleLyricSO", menuName = "RapBattle/RapBattle/RapBattleLyricSO", order = 1)]
public class RapBattleLyricSO : ScriptableObject
{
    public List<Levels> leveldata;
}
