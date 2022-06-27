using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class ObjectShake : MonoBehaviour
{
    public float shakeAmount = 20f;
    public float randomness = 3f;

    [Button]
    public void Shake()
    {
        transform.DOShakePosition(0.5f, shakeAmount, 10, randomness, false);
    }
}
