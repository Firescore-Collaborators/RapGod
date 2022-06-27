using UnityEngine;
using System;

namespace PrisonControl
{
    public class WardenSnapshotTaker : MonoBehaviour
    {
        int resWidth = 256;
        int resHeight = 256;

        Camera snapCam;

        public Action <byte[]> PhotoCaptured;

        void Awake()
        {
            snapCam = GetComponent<Camera>();

            if (snapCam.targetTexture == null)
            {
                snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24);
            }
            else
            {
                resWidth = snapCam.targetTexture.width;
                resHeight = snapCam.targetTexture.height;
            }

            gameObject.SetActive(false);
        }

        public void TakeSnapShot()
        {
            gameObject.SetActive(true);
        }

        void LateUpdate()
        {
            if (gameObject.activeInHierarchy)
            {
                Snap();
            }
        }

        void Snap()
        {
            Texture2D snapShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            snapCam.Render();

            RenderTexture.active = snapCam.targetTexture;
            snapShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

            byte[] bytes = snapShot.EncodeToPNG();

            PhotoCaptured?.Invoke(bytes);
            gameObject.SetActive(false);
        }
    }
}
