using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public MonoBehaviour callback;
    public string functionName;

    public void OnEnd()
    {
        callback.Invoke(functionName, 0);
    }

  
}
