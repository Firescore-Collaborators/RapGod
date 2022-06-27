using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;


namespace PrisonControl
{
    [CreateAssetMenu(fileName = "PrisonYard", menuName = "PrisonControl/PrisonYard", order = 51)]
    public class PrisonYard_SO : ScriptableObject
    {
        public string[] scenarioInfo;
        public GameObject [] pf_scenario;
        public SpawnPos[] spawnPoses;

        public enum SpawnPos
        {
            spawnPos1,
            spawnPos2,
            spawnPos3,
            spawnPos_ladder,
            spawnPos5,
            spawnPosStairs,
        }

    }
}
