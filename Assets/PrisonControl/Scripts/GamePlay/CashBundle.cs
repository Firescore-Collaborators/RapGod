using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class CashBundle : MonoBehaviour
    {
        [SerializeField]
        private GameObject particleEffect;

        public void ActivateParticleEffect()
        {
            particleEffect.SetActive(true);
        }
    }
}
