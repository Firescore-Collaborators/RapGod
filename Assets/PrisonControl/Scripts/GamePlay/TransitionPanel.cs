using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PrisonControl
{
    public class TransitionPanel : MonoBehaviour
    {
        Action callback;

        public void DoTransition(Action _callback = null)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<Animator>().Play("Transition");
            callback = _callback;
        }

        public void OnTransitionDone()
        {
            callback?.Invoke();
            callback = null;

        }
    }
}
