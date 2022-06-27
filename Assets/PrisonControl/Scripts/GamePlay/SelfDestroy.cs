using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField]
    private float destroyTime;

    void OnEnable()
    {
        Timer.Delay(destroyTime, () =>
        {
            Destroy(gameObject);
        });
    }
}
