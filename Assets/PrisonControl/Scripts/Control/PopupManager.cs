using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    public enum PopupId
    {
        Items,
        Confirmation
    }

    public class PopupManager : MonoBehaviour
    {
        public void PushPopup(PopupId popupId)
        {
            Popup popup = mPopupRegistry[popupId];
            Debug.Assert(popup != null);
            Debug.Assert(!mPopupStack.Contains(popup));

            mHolder.SetActive(true);
            if (mPopupStack.Count > 0)
                mPopupStack.Peek().gameObject.SetActive(false);

            mPopupStack.Push(popup);
            popup.gameObject.SetActive(true);
        }

        public void PopPopup()
        {
            Debug.Assert(mPopupStack.Count > 0);
            mPopupStack.Peek().gameObject.SetActive(false);
            mPopupStack.Pop();

            if (mPopupStack.Count > 0)
                mPopupStack.Peek().gameObject.SetActive(true);
            else
                mHolder.SetActive(false);
        }

        public bool OnBackButtonPressed()
        {
            if (mPopupStack.Count > 0)
                return mPopupStack.Peek().OnBackButtonPressed();
            else
                return false;
        }

        public void RegisterPopups(params (PopupId, Popup)[] popups)
        {
            foreach (var (popupId, popup) in popups)
            {
                popup.gameObject.SetActive(false);
                mPopupRegistry.Add(popupId, popup);
            }
        }

        public static PopupManager Instance
        {
            get
            {
                Debug.Assert(smInstance != null);
                return smInstance;
            }
        }

        private void Awake()
        {
            Debug.Assert(smInstance == null);
            smInstance = this;

            mHolder.SetActive(false);
        }

        private Dictionary<PopupId, Popup> mPopupRegistry = new Dictionary<PopupId, Popup>();
        private Stack<Popup> mPopupStack = new Stack<Popup>();

        [SerializeField]
        private GameObject mHolder;

        private static PopupManager smInstance;
    }
}
