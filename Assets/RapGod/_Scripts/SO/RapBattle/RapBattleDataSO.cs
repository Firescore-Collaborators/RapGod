using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RapAnimation
{
    HandRaise,
}
public enum RapPose
{
    layingDown,
    WaveHipHop
}

[CreateAssetMenu(fileName = "RapBattleDataSO", menuName = "RapBattle/RapBattle/RapBattleDataSO", order = 1)]
public class RapBattleDataSO : ScriptableObject
{
    public GameObject enemyCharacter;
    public EnvironmentSO environment;
    public RapBattleLyricSO rapBattleLyricSO;
    public InputSequenceSO inputSequence;
    public RapAnimation rapAnimation;
    public RapPose rapPose;
    public string punchLine;
    public float tapSmashLimit = 20f;
    public float maxAnimationSpeed = 3f;

}

//Cherry Pick
