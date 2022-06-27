using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepState : MonoBehaviour
{
    public GameObject stepManager;
    public virtual void OnStepStart()
    {
        stepManager.SetActive(true);
    }

    public virtual void OnStepEnd()
    {
        stepManager.SetActive(false);
    }
}
