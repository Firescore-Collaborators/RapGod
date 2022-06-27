using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace PrisonControl
{
    public class JailEntryUi : MonoBehaviour
    {
        public Action<bool> _OnPassClicked;
        public Action<string> _OnArrestClicked;

        [SerializeField]
        private GameObject tutorialLoop, panelCheck, panelAllow, panelArrest;

        [SerializeField]
        private Text txt_allow, txt_punishmentType1, txt_punishmentType2;

        [SerializeField]
        private Image img_emoji1, img_emoji2;

      

        private void OnEnable()
        {
            tutorialLoop.SetActive(false);
        }

        public void SetupPunishment(JailEntry_SO jailEntry_SO)
        {

            txt_punishmentType1.GetComponent<PunishmentVal>()._punishmentVal = jailEntry_SO.punishmentType1.ToString();
         //   txt_punishmentType2.GetComponent<PunishmentVal>()._punishmentVal = (int)jailEntry_SO.punishmentType2;

            //txt_allow.text = ""+ jailEntry_SO.copResponse;
            txt_allow.text = "Allow";

            txt_punishmentType1.text = ""+jailEntry_SO.punishmentType1;
         //   txt_punishmentType2.text = ""+jailEntry_SO.punishmentType2;

            img_emoji1.sprite = Resources.Load("PunishmentEmoji/" + jailEntry_SO.punishmentType1, typeof(Sprite)) as Sprite;
        //    img_emoji2.sprite = Resources.Load("PunishmentEmoji/" + jailEntry_SO.punishmentType2, typeof(Sprite)) as Sprite;

        }

        public void OnNextClicked()
        {
            panelAllow.SetActive(false);
            _OnPassClicked?.Invoke(true);
        }

        public void OnArrestClicked(Text txt_arrestType)
        {
            panelArrest.SetActive(false);
            _OnArrestClicked?.Invoke(txt_arrestType.GetComponent<PunishmentVal>()._punishmentVal);
        }

        public void ActivateLoop(bool activate)
        {
            tutorialLoop.SetActive(activate);
        }

        public void ActivateCheckPanel(bool activate)
        {
            panelCheck.SetActive(activate);
        }

        public void ActivateAllowPanel(bool activate)
        {
            panelAllow.SetActive(activate);
        }

        public void ActivateArrestPanel(bool activate)
        {
            panelArrest.SetActive(activate);
        }
    }
}
