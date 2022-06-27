using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(PopupManager))]
    public class PopupManagerConfig : MonoBehaviour
    {
        private void Awake()
        {
            PopupManager manager = GetComponent<PopupManager>();
            manager.RegisterPopups(
                (PopupId.Items, mItemsPopup),
                (PopupId.Confirmation, mConfirmationPopup)
            );
        }

        [SerializeField]
        Popup mItemsPopup;

        [SerializeField]
        Popup mConfirmationPopup;
    }
}
