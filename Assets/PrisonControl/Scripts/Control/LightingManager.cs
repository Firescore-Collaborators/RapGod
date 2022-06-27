using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class LightingManager : MonoBehaviour
    {

        [SerializeField]
        private GameObject avatarLights, gameLights;

        public void ActivateGameLights()
        {
            avatarLights.SetActive(false);
            gameLights.SetActive(true);
        }

        public void ActivateAvatarLights()
        {
            avatarLights.SetActive(true);
            gameLights.SetActive(false);
        }
    }
}
