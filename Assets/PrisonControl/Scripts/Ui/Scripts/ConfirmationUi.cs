using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public class ConfirmationUi : Popup
    {
        public void ShowConfirmation(System.Action positiveCallback, System.Action negativeCallback)
        {
            mPositiveCallback = positiveCallback;
            mNegativeCallback = negativeCallback;

            PopupManager.Instance.PushPopup(PopupId.Confirmation);
        }

        public override bool OnBackButtonPressed()
        {
            DispatchNegativeResponse();
            return true;
        }

        public void _YesClicked()
        {
            DispatchPositiveResponse();
        }

        public void _NoClicked()
        {
            DispatchNegativeResponse();
        }

        private void DispatchPositiveResponse()
        {
            System.Action callback = mPositiveCallback;
            ResetCallbacks();
            callback?.Invoke();
        }

        private void DispatchNegativeResponse()
        {
            System.Action callback = mNegativeCallback;
            ResetCallbacks();
            callback?.Invoke();
        }

        private void ResetCallbacks()
        {
            mPositiveCallback = null;
            mNegativeCallback = null;
        }

        private System.Action mPositiveCallback;
        private System.Action mNegativeCallback;
    }
}
