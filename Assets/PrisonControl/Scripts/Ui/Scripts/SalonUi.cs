using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrisonControl
{
    public class SalonUi : MonoBehaviour
    {
        public event System.Action OnInteriorUpgradeClicked;
        public event System.Action OnWallUpgradeClicked;
        public event System.Action OnFloorUpgradeClicked;
        public event System.Action OnBackClicked;

        public void _OnInteriorClicked()
        {
            OnInteriorUpgradeClicked?.Invoke();
        }

        public void _OnWallClicked()
        {
            OnWallUpgradeClicked?.Invoke();
        }

        public void _OnFloorClicked()
        {
            OnFloorUpgradeClicked?.Invoke();
        }

        public void _OnBackClicked()
        {
            OnBackClicked?.Invoke();
        }

        private void Awake()
        {
            Progress.Instance.OnSalonUpgraded += UpdateView;
            Progress.Instance.OnCurrencyChanged += UpdateView;
        }

        private void OnDestroy()
        {
            Progress.Instance.OnSalonUpgraded -= UpdateView;
            Progress.Instance.OnCurrencyChanged -= UpdateView;
        }

        private void OnEnable()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            bool canAffordInterior = Progress.Instance.CanAfford(Progress.Instance.GetInteriorPrice());
            mInteriorLevel.color = canAffordInterior ? Color.white : Color.gray;
            mInteriorLevel.text = $"Level {Progress.Instance.InteriorLevel}";
            mInteriorPrice.text = $"Price ${Progress.Instance.GetInteriorPrice()}";

            bool canAffordWall = Progress.Instance.CanAfford(Progress.Instance.GetWallPrice());
            mWallLevel.color = canAffordWall ? Color.white : Color.gray;
            mWallLevel.text = $"Level {Progress.Instance.WallLevel}";
            mWallPrice.text = $"Price ${Progress.Instance.GetWallPrice()}";

            bool canAffordFloor = Progress.Instance.CanAfford(Progress.Instance.GetFloorPrice());
            mFloorLevel.color = canAffordFloor ? Color.white : Color.gray;
            mFloorLevel.text = $"Level {Progress.Instance.FloorLevel}";
            mFloorPrice.text = $"Price ${Progress.Instance.GetFloorPrice()}";
        }

        [SerializeField]
        private Text mInteriorLevel;

        [SerializeField]
        private Text mInteriorPrice;

        [SerializeField]
        private Text mWallLevel;

        [SerializeField]
        private Text mWallPrice;

        [SerializeField]
        private Text mFloorLevel;

        [SerializeField]
        private Text mFloorPrice;
    }
}
