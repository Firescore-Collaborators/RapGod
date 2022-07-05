using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LyricListSO", menuName = "RapBattle/LyricsGame/LyricListSO")]
public class LyricListSO : ScriptableObject
{
    public List<Lyrics_SO> lyricList = new List<Lyrics_SO>();
}
