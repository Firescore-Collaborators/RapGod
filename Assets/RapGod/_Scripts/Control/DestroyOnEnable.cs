 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnEnable : MonoBehaviour
{
    public float timer = 5f;
    void OnEnable()
    {
        Timer.Delay(timer, () =>
        {
            Destroy(gameObject);
        });
    }
}
