using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrisonControl
{
    public class HeaderUi : MonoBehaviour
    {
        private void Awake()
        {
            UpdateText();

            Progress.Instance.OnCurrencyChanged += UpdateText;
        }

        private void OnDestroy()
        {
            Progress.Instance.OnCurrencyChanged -= UpdateText;
        }

        private void UpdateText()
        {
            mCurrencyText.text = $"${Progress.Instance.Currency}";
        }

        [SerializeField]
        Text mCurrencyText;
    }
}
