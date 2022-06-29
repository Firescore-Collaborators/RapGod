using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PrisonControl
{
    public class AvatarSelectionUi : MonoBehaviour
    {
        public event System.Action OnBackClicked;
        public event System.Action _OnGenderSelectionDone;
        public event System.Action _OnAvatarSelectionDone;

        public event System.Action _OnAvatarNext;
        public event System.Action _OnAvatarPrevious;

        private void OnEnable()
        {
            btnPrev.interactable = false;
            Progress.Instance.AvatarType = 1;
        }

        public void OnGenderSelected()
        {
            Debug.Log("On gender selected");
            _OnGenderSelectionDone?.Invoke();
            head1.SetActive(false);
        }

        public void OnAvatarSelected()
        {
            Debug.Log("On Avatar selected");
            responseMsg.SetActive(true);

            Timer.Delay(2, () =>
            {
                _OnAvatarSelectionDone?.Invoke();
                Progress.Instance.IsAvatarSelected = true;
            });

        }

        public void OnAvatarNextClicked()
        {
            _OnAvatarNext?.Invoke();

            if (Progress.Instance.AvatarType >= 3)
            {
                btnNext.interactable = false;
            }
            btnPrev.interactable = true;

        }

        public void OnAvatarPreviousClicked()
        {
            _OnAvatarPrevious?.Invoke();

            if (Progress.Instance.AvatarType <= 1)
            {
                btnPrev.interactable = false;
            }
            btnNext.interactable = true;
        }

        public void ActivateGenderSelection()
        {
            genderSelectionPanel.SetActive(true);
            uniformSelectionPanel.SetActive(false);
        }

        public void ActivateUniformSelection()
        {
            genderSelectionPanel.SetActive(false);

            Timer.Delay(0.5f, () =>
            {
                uniformSelectionPanel.SetActive(true);
            });
        }

        [SerializeField]
        private GameObject genderSelectionPanel, uniformSelectionPanel, head1, responseMsg;


        [SerializeField]
        private Button btnPrev, btnNext;
    }

}
