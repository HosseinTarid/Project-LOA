using UnityEngine;
using UI.Core;
using System;

namespace UI.MainHUD
{

    public class MainHUDState : ScreenState<MainHUDState, MainHUDView>
    {

        public override void Initialize()
        {
            base.Initialize();

            view.InventoryBtn.onClick.AddListener(ShowInventory);
            view.OnlineMarketBtn.onClick.AddListener(ShowMarketPlace);
        }

        private void ShowInventory()
        {
            Inventory.InventoryState.Instance.Show();
        }
        private void ShowMarketPlace()
        {

        }

        public void UpdateLumenText(int value)
        {
            view.LumenTxt.text = value.ToString();
        }
        public void UpdatePlatiniumText(int value)
        {
            view.PlatiniumTxt.text = value.ToString();
        }

    }

}