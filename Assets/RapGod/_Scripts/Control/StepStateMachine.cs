using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StepStateMachine 
{
    
    public static Dictionary<StepType,string> stepStates = new Dictionary<StepType, string>{
        
        {StepType.shirtColor, "ShirtColorStepState"},
        {StepType.hairSelect,"HairSelectStepState"},
        {StepType.levelEnd, "LevelCompleteStepState"}
    };


}
