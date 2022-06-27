using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDeactivate : MonoBehaviour
{
    public bool selfDeactivate;

    [SerializeField]
    private float delayTimer;

    void OnEnable()
    {
        if (!selfDeactivate)
            return;

        Timer.Delay(delayTimer, () =>
        {
            Disabel();
        });
    }

    public void Disabel()
    {
        Debug.Log("---- deactivate it");
        gameObject.SetActive(false);
    }
}
