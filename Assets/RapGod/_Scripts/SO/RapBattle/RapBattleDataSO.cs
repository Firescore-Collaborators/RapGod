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
public enum rapCameras{
    defaultCam = 0,
    Camera1 = 1,
    Camera2 = 2,
    Camera3 = 3,
    Camera4 = 4,
    Camera5 = 5,
    Camera6 = 6,
    Camera7 = 7,
    Camera8 = 8,
    Camera9 = 9,
    Camera10 = 10,
    
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
    public List<rapCameras> rapCameras = new List<rapCameras>();
    public string punchLine;
    public float tapSmashLimit = 20f;
    public float maxAnimationSpeed = 3f;

}
