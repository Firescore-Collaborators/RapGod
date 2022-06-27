using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

namespace PrisonControl
{
    public class CCTVMonitorUI : MonoBehaviour
    {
        public Action _OnPassClicked;
        public Action _OnSuspiciousClicked;

        public Action _OnIgnoreClicked;
        public Action _OnPunishmentClicked;


        [SerializeField]
        private GameObject panelDecision, panelPunishment;

        public Text punishmentText;
        public Image img_emoji;

        private void OnEnable()
        {
            panelDecision.SetActive(false);
            panelPunishment.SetActive(false);
        }

        public void ShowDecisionPanel()
        {
            panelPunishment.SetActive(false);
            Timer.Delay(1, () =>
            {
                panelDecision.SetActive(true);
            });
        }

        public void ShowPunishmentPanel()
        {
            panelDecision.SetActive(false);
            Timer.Delay(1, () =>
            {
                panelPunishment.SetActive(true);
            });
        }

        public void Pass()
        {
            _OnPassClicked?.Invoke();
        }

        public void Suspicious()
        {
            _OnSuspiciousClicked?.Invoke();
            ShowPunishmentPanel();
        }

        public void Ignore()
        {
            _OnIgnoreClicked?.Invoke();
            panelPunishment.SetActive(false);
        }

        public void Punishment()
        {
            _OnPunishmentClicked?.Invoke();
            panelPunishment.SetActive(false);
        }
    }
}
