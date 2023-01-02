using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace UI
{
    public class LandPanel : UIBasePanel
    {
        [Space]
        [SerializeField]
        Pool<LandPanelItem> landItemPool = new Pool<LandPanelItem>();
        [SerializeField]
        ScrollRect landSR;

        List<LandPanelItem> landItems = new List<LandPanelItem>();
        bool isBuildingTabActive = false;

        public override void Enter(Action onEnterDoneCB = null)
        {
            base.Enter(onEnterDoneCB);

            DisableItems();

            for (int i = 0; i < LOAGameManager.Instance.playerProfile.lands.Count; i++)
            {
                LandPanelItem newItem = landItemPool.GetActive;
                newItem.SetData(LOAGameManager.Instance.playerProfile.lands[i]);
                landItems.Add(newItem);
            }

            landSR.horizontalNormalizedPosition = 0;
        }

        void DisableItems()
        {
            foreach (var item in landItems)
            {
                item.gameObject.SetActive(false);
            }

            landItems.Clear();
        }
    }
}
