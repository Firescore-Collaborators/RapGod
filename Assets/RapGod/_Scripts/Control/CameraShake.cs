using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.3f;
    public float shakeAmount = 0.7f;
    Tweener tweener;

    [Button]
    public void Shake()
    {
        if(tweener !=null)
            if(tweener.IsPlaying()) return;

        tweener = transform.DOShakePosition(shakeDuration, shakeAmount);
    }

    public void Shake(Transform target)
    {
        if(tweener !=null)
            if(tweener.IsPlaying()) return;

        tweener = target.DOShakePosition(shakeDuration, shakeAmount);
    }
}
