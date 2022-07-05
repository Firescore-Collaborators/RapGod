using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LyricListSO", menuName = "RapGod/LyricList/LyricListSO")]
public class LyricListSO : ScriptableObject
{
    public List<Lyrics_SO> lyricList = new List<Lyrics_SO>();
}
