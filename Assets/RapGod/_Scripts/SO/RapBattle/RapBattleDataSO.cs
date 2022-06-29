using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RapBattleDataSO", menuName = "RapBattle/RapBattle/RapBattleDataSO", order = 1)]
public class RapBattleDataSO : ScriptableObject
{
    public GameObject enemyCharacter;
    public RapBattleLyricSO rapBattleLyricSO;
}
