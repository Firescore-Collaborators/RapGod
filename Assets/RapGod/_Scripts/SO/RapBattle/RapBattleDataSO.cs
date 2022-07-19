using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RapEndAnimation
{
    HandRaise,
}
public enum RapPose
{
    layingDown,
    Dying,
    WaveHipHop,
    KissMan
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

public enum RapAnimation
{
    Rapping_Default,
    Rapping1,
    Rapping2,
    Threatening,
    StandingArgue1,
    StandingArgue2,
    StandingArgue3,
    StandingArgue4,
    Singing,
    Defeated,
    ArmStretching,
    Dying,
    Pointing,
    SpellCast,
    SurpriseUppercut,
    WideArmSpellCast,

}

[System.Serializable]
public class RapAnimations
{
    public RapAnimation playerAnim;
    public RapAnimation enemyAnim;
}

[System.Serializable]
public class RapPoseData
{
    public RapPose playerSuccess;
    public RapPose enemyFail;
    public RapPose playerFail;
    public RapPose enemySuccess;
}

[CreateAssetMenu(fileName = "RapBattleDataSO", menuName = "RapBattle/RapBattle/RapBattleDataSO", order = 1)]
public class RapBattleDataSO : ScriptableObject
{
    public GameObject enemyCharacter;
    public GameObject girlCharacter;
    public EnvironmentSO environment;
    public RapBattleLyricSO rapBattleLyricSO;
    public InputSequenceSO inputSequence;
    public RapEndAnimation rapEndAnimation;
    public RapPoseData rapPoses;
    public List<RapAnimations> rapAnimations = new List<RapAnimations>();
    public List<rapCameras> rapCameras = new List<rapCameras>();
    public string punchLine;
    public string youtubeVideoTitle = "LATEST RAP SONG RELEASED";
    public float tapSmashLimit = 20f;
    public float maxAnimationSpeed = 3f;

}

//Cherry Pick
