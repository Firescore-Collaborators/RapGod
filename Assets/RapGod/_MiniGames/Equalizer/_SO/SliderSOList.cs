using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slider", menuName = "RapGod/Equalizer/Slider", order = 51)]
public class SliderSOList : ScriptableObject
{
    public List<int> reading = new List<int>();
}
