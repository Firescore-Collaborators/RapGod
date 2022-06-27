using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public abstract class Popup : MonoBehaviour
    {
        public virtual bool OnBackButtonPressed()
        {
            return false;
        }
    }
}
