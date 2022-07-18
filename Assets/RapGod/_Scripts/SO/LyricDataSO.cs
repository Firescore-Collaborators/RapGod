using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LyricListSO", menuName = "RapBattle/LyricsGameNew/LyricListSO")]
public class LyricDataSO : ScriptableObject
{
    public List<Lyric_SO2> lyricsList = new List<Lyric_SO2>();
}
