using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;
using System.IO;

namespace PrisonControl
{
    public class MugshotUi : MonoBehaviour
    {
        public Action<bool> responseClicked;
        public Action photoCaptureInitiated;
        public Action OnCloseSnapShot;
        public Action OnCloseRegister;


        [SerializeField]
        private Text txt_positiveBtn, txt_negetiveBtn;

        [SerializeField]
        private GameObject btnClick, panel, panelSnapShot, panelRegister, registerPad, mugshotHeader;

        [SerializeField]
        private RawImage rawImg_snapShot;

        public string path;

        [SerializeField]
        private TextMeshPro [] txt_prisonerName;

        [SerializeField]
        private MeshRenderer[] img_prisonerSnaps;

        [SerializeField]
        private GameObject handTut1, handTut2;

        [SerializeField]
        private AudioClip m_ShotSound;

        [SerializeField]
        private GameObject flash;

        [SerializeField]
        private RespondMessage respondMessage;


        private void OnEnable()
        {
            handTut1.SetActive(false);
            handTut2.SetActive(false);
            mugshotHeader.SetActive(true);

            ToggleTut1(true);
        }

        public void BtnResponse(bool isPositive)
        {
            responseClicked?.Invoke(isPositive);

            btnClick.SetActive(true);

            ToggleTut1(false);
            ToggleTut2(true);
        }

        void ToggleTut1(bool activate)
        {
            if (!Progress.Instance.IsMugShotTutDone)
                handTut1.SetActive(activate);
        }

        void ToggleTut2(bool activate)
        {
            if (!Progress.Instance.IsMugShotTutDone)
                handTut2.SetActive(activate);
        }

        public void Setup(Mugshot_SO mugshotInfo, int prisonersIndex)
        {
            txt_positiveBtn.text = "" + mugshotInfo.positiveResponse[prisonersIndex];
            txt_negetiveBtn.text = "" + mugshotInfo.negetiveResponse[prisonersIndex];
        }

        public void Capture()
        {
            respondMessage.ShowCorrectMsg();

            flash.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(m_ShotSound);

            Progress.Instance.IsMugShotTutDone = true;
            handTut2.SetActive(false);

            photoCaptureInitiated?.Invoke();
        }

        public void ShowUi()
        {
            panel.SetActive(true);
        }

        public void ShowSnapShot(int prisonerNo, byte[] bytes)
        {
            panel.SetActive(false);
            panelSnapShot.SetActive(true);

            var texture = new Texture2D(73, 73);
            texture.LoadImage(bytes);

            rawImg_snapShot.texture = texture;
            img_prisonerSnaps[prisonerNo - 1].material.SetTexture("_BaseMap", texture); ;

            Invoke("CloseSnapShot", 2);

        }

        public void CloseSnapShot()
        {
            CancelInvoke("CloseSnapShot");

            panelSnapShot.SetActive(false);
            OnCloseSnapShot?.Invoke();
        }

        public void ShowRegisterPage(Mugshot_SO mugshot_SO)
        {

            panelRegister.SetActive(true);
            registerPad.SetActive(true);

            mugshotHeader.SetActive(false);

            for (int i = 0; i < 3; i++)
            {
                txt_prisonerName[i].text = mugshot_SO.prisoners[i].prisoner_name;
            }
        }

        public void CloseRegister()
        {
            panelRegister.SetActive(false);
            registerPad.SetActive(false);

            OnCloseRegister?.Invoke();
        }
    }
}
