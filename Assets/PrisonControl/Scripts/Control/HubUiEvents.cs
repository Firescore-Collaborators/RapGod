using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class HubUiEvents : MonoBehaviour
    {
        public event System.Action OnPlayClicked;
        public event System.Action OnNailShapesClicked;
        public event System.Action OnToolsClicked;
        public event System.Action OnSalonClicked;

        public void PlayClicked()
        {
            OnPlayClicked?.Invoke();
        }

        public void NailShapesClicked()
        {
            OnNailShapesClicked?.Invoke();
        }

        public void ToolsClicked()
        {
            OnToolsClicked?.Invoke();
        }

        public void SalonClicked()
        {
            OnSalonClicked?.Invoke();
        }
    }
}
