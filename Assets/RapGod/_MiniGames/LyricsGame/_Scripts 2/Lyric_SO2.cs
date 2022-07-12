using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LyricsGameNew", menuName = "RapBattle/LyricsGameNew/LyricsFill", order = 51)]
public class Lyric_SO2 : ScriptableObject
{
    public string[] Lyrics;
    public List<string> option = new List<string>();
  
}
