using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Assets.Script.Services.API_Service.Game.User;


namespace UI
{
    public class BuildPanel : UIBasePanel
    {
        [Space]
        [SerializeField]
        Pool<BuildPanelItem> buildingItemPool = new Pool<BuildPanelItem>();
        [SerializeField]
        ScrollRect buildingSR;
        [SerializeField]
        GameObject noBuildingText;
        [SerializeField]
        Pool<BuildPanelItem> decorationItemPool = new Pool<BuildPanelItem>();
        [SerializeField]
        ScrollRect decorationSR;
        [SerializeField]
        GameObject noDecorationText;
        [SerializeField]
        Image buildingTab;
        [SerializeField]
        Image decorationTab;
        [SerializeField]
        Sprite enabledSprite;
        [SerializeField]
        Sprite disabledSprite;

        List<BuildPanelItem> buildingItems = new List<BuildPanelItem>();
        List<BuildPanelItem> decorationItems = new List<BuildPanelItem>();
        bool isBuildingTabActive = false;

        public override void Enter(Action onEnterDoneCB = null)
        {
            base.Enter(onEnterDoneCB);

            OnBuildingTabClick();

            DisableItems();

            Dictionary<BuildingCard, int> dict = new Dictionary<BuildingCard, int>();
            foreach (var building in LOAGameManager.Instance.playerProfile.availableBuildings)
            {
                BuildingCard card = new BuildingCard(building.buildingInfo.type.type, building.buildingInfo.level);
                if (!dict.ContainsKey(card))
                    dict.Add(card, 1);
                else
                    dict[card]++;
            }

            foreach (var card in dict)
            {
                BuildPanelItem newItem = buildingItemPool.GetActive;
                newItem.SetData(card.Key.type, card.Key.level, card.Value, () => OnItemClick(card.Key));
                buildingItems.Add(newItem);
            }

            noBuildingText.SetActive(dict.Count == 0);
            buildingSR.horizontalNormalizedPosition = 0;

            for (int i = 0; i < 0; i++)
            {
                BuildPanelItem newItem = decorationItemPool.GetActive;
                //newItem.SetData(null);
                decorationItems.Add(newItem);
            }

            decorationSR.horizontalNormalizedPosition = 0;
            noDecorationText.SetActive(true);
        }

        void DisableItems()
        {
            foreach (var item in buildingItems)
            {
                item.gameObject.SetActive(false);
            }

            buildingItems.Clear();

            foreach (var item in decorationItems)
            {
                item.gameObject.SetActive(false);
            }

            decorationItems.Clear();
        }

        public void OnBuildingTabClick()
        {
            if (isBuildingTabActive)
                return;
            isBuildingTabActive = true;

            buildingTab.sprite = enabledSprite;
            decorationTab.sprite = disabledSprite;
            buildingSR.gameObject.SetActive(true);
            decorationSR.gameObject.SetActive(false);
        }

        public void OnDecorationTabClick()
        {
            if (!isBuildingTabActive)
                return;
            isBuildingTabActive = false;

            buildingTab.sprite = disabledSprite;
            decorationTab.sprite = enabledSprite;
            buildingSR.gameObject.SetActive(false);
            decorationSR.gameObject.SetActive(true);
        }

        void OnItemClick(BuildingCard card)
        {
            var building = LOAGameManager.Instance.playerProfile.availableBuildings.Find(x => x.buildingInfo.type.type == card.type && x.buildingInfo.level == card.level);
            bool result = LOAGameManager.Instance.buildingsManager.CheckForPlacingNewBilding(building.buildingInfo.type.type, building.buildingInfo.level, building.id);

            if (result)
                LOAGameManager.Instance.uIManager.ChangePanel(PanelType.BuildingPlacement);
            else
            {
                //Debug.LogError("Not enough Space for this building.");
                PopUpPanel.SetPopUpData("Not enough Space for this building.", "OK", true, () => OnBackClick(), null);
                LOAGameManager.Instance.uIManager.ChangePanel(PanelType.PopUp);
            }
        }

        struct BuildingCard
        {
            public BuildingType type;
            public int level;

            public BuildingCard(BuildingType type, int level)
            {
                this.type = type;
                this.level = level;
            }
        }
    }
}
