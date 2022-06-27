using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class CopHandler : MonoBehaviour
    {
        public Avatar [] avatar;

        private Avatar selectedAvatar;

        void OnEnable()
        {
            selectedAvatar = avatar[Progress.Instance.AvatarGender];
            selectedAvatar.gameObject.SetActive(true);
            selectedAvatar.SetUniform();
        }

        public Avatar ReturnSelectedAvatar()
        {
            selectedAvatar = avatar[Progress.Instance.AvatarGender];
            return selectedAvatar;
        }
    }
}
