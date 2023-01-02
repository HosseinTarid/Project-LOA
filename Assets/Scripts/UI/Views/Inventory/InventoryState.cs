using UnityEngine;
using UI.Core;
using System;

namespace UI.Inventory
{

    public class InventoryState : ScreenState<InventoryState, InventoryView>
    {

        public override void Initialize()
        {
            base.Initialize();
            ShowBuildingList();

            view.CloseBtn.onClick.AddListener(CloseInventory);
            view.BuildingTabBtn.onClick.AddListener(ShowBuildingList);
            view.DecorationTabBtn.onClick.AddListener(ShowDecorationList);
        }

        private void CloseInventory()
        {
            Hide();
        }

        private void ShowBuildingList()
        {
            view.BuildingListGO.SetActive(true);
            view.DecorationListGO.SetActive(false);
        }

        private void ShowDecorationList()
        {
            view.BuildingListGO.SetActive(false);
            view.DecorationListGO.SetActive(true);
        }

    }

}