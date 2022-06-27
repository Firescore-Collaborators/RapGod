using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PrisonControl
{
    public class UIElements : MonoBehaviour
    {
        public static UIElements instance;
        public static event System.Action OnArrest;
        public static event System.Action OnSuspicious;
        public static event System.Action<bool> OnArrestGuilty;
        public static event System.Action<bool> OnAllowedCorrect;

        public static event System.Action OnAllow;
        public static event System.Action OnNextDialogue;

        public static event System.Action OnButtonClicked;
        public GameObject choicePanelSus;
        public GameObject choicePanelArrest;

        [SerializeField]
        private Text txt_allow;


        [SerializeField]
        private GameObject btnAllow, btnContinue;

        public bool visitorOver;
        public bool isGuilty;

        private void Awake() {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        void OnEnable()
        {
          //  VisitorManager.OnPlayDialogue += ChoicePanelOn;
            VisitorManager.OnSuspiciousStart += SwitchPanel;
            VisitorManager.OnTextCompleted += ChoicePanelOn;

            OnButtonClicked += ChoicePanelOff;
        }

        void OnDisable()
        {
         //   VisitorManager.OnPlayDialogue -= ChoicePanelOn;
            VisitorManager.OnSuspiciousStart -= SwitchPanel;
            VisitorManager.OnTextCompleted -= ChoicePanelOn;

            OnButtonClicked -= ChoicePanelOff;
        }

        public void Suspicious()
        {
            OnSuspicious?.Invoke();
            //choicePanelSus.SetActive(false);
            //choicePanelArrest.SetActive(true);
            OnButtonClicked?.Invoke();
            visitorOver = true;
        }
    
        public void Arrest()
        {
            //if (isGuilty)
            //{
                OnArrestGuilty?.Invoke(isGuilty);
            //}
            OnButtonClicked?.Invoke();
            OnArrest?.Invoke();
        }

        public void Continue()
        {
            choicePanelSus.SetActive(false);

            if (visitorOver)
            {
                //if (!isGuilty)
                //{
                    OnAllowedCorrect?.Invoke(isGuilty);
                //}
                OnButtonClicked?.Invoke();
                OnAllow?.Invoke();
            }
            else
            {
                OnNextDialogue?.Invoke();
            }
        }

        public void ChoicePanelOn(int index)
        {
            if (index == 0)
            {
                btnAllow.SetActive(false);
                btnContinue.SetActive(true);
            }
            else
            {
                btnAllow.SetActive(true);
                btnContinue.SetActive(false);
            }

            choicePanelSus.SetActive(true);
        }

        public void ChoicePanelOff()
        {
            choicePanelArrest.SetActive(false);
            choicePanelSus.SetActive(false);
        }

        void SwitchPanel()
        {
            choicePanelSus.SetActive(false);
            choicePanelArrest.SetActive(true);
        }
    }
}
