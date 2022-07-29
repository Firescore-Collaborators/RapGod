using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum HandShakeType
{
    SnakeHipHop,
    MagicAttack1,
    AfricanNoodle,
    ThrillerPart3,
    StrutWalking,
    BodyWave,
    RibsPop,
    RunningMan,
    GuitarPlaying,
    BlowKiss,
    Kiss,
    Happy1,
}

[System.Serializable]
public class HandShakeEnd
{
    public HandShakeType playerEnd;
    public HandShakeType enemyEnd;
}

[CreateAssetMenu(fileName = "HandShakeSO", menuName = "RapBattle/HandShake/HandShakeSO")]
public class HandShakeSO : ScriptableObject
{
    public List<HandShakeType> player = new List<HandShakeType>();
    public List<HandShakeType> enemy = new List<HandShakeType>();
    public HandShakeEnd handShakeEnd;
    public InputSequenceSO inputSequence;
    public float multiTapLimit = 10f; 
    public float animationMaxSpeed = 3f;
}
