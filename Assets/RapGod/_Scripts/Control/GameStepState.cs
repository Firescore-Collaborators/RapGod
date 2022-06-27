using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStepState : StepState
{
    public override void OnStepStart()
    {
        UIElements.instance.StepStart();
        stepManager.SetActive(true);
    }

    public override void OnStepEnd()
    {
        stepManager.SetActive(false);
        UIElements.instance.StepComplete();
    }
}
