using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrisonControl
{
    [RequireComponent(typeof(GameFlow))]
    public class JailState : State
    {
        private void Awake()
        {
            mGameFlow = GetComponent<GameFlow>();
            mSalonUi.gameObject.SetActive(false);

            mSalonUi.OnInteriorUpgradeClicked += OnInteriorUpgradeClicked;
            mSalonUi.OnWallUpgradeClicked += OnWallUpgradeClicked;
            mSalonUi.OnFloorUpgradeClicked += OnFloorUpgradeClicked;
            mSalonUi.OnBackClicked += GoBack;
        }

        private void OnDestroy()
        {
            mSalonUi.OnBackClicked -= GoBack;
        }

        public override void OnEnter()
        {
            mSalonUi.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            mSalonUi.gameObject.SetActive(false);
        }

        public override bool OnBackButtonPressed()
        {
            GoBack();
            return true;
        }

        private void GoBack()
        {
            mGameFlow._SwitchState(GameState.Hub);
        }

        private void OnInteriorUpgradeClicked()
        {
            int price = Progress.Instance.GetInteriorPrice();
            if (Progress.Instance.CanAfford(price))
            {
                ++Progress.Instance.InteriorLevel;
                Progress.Instance.Currency -= price;
            }
        }

        private void OnWallUpgradeClicked()
        {
            int price = Progress.Instance.GetWallPrice();
            if (Progress.Instance.CanAfford(price))
            {
                ++Progress.Instance.WallLevel;
                Progress.Instance.Currency -= price;
            }
        }

        private void OnFloorUpgradeClicked()
        {
            int price = Progress.Instance.GetFloorPrice();
            if (Progress.Instance.CanAfford(price))
            {
                ++Progress.Instance.FloorLevel;
                Progress.Instance.Currency -= price;
            }
        }

        private GameFlow mGameFlow;

        [SerializeField]
        SalonUi mSalonUi;
    }
}
