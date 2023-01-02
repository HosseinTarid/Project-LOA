using UnityEngine;
using UI.Core;
using System;

namespace UI.BuildingInfoHUD
{

    public class BuildingInfoHUDState : ScreenState<BuildingInfoHUDState, BuildingInfoHUDView>
    {

        private Action _infoAction;
        private Action _featureAction;
        private Action _destroyAction;
        private Action _repairAction;

        public override void Initialize()
        {
            base.Initialize();

            view.InfoButton.onClick.AddListener(ShowInfo);
            view.FeatureButton.onClick.AddListener(ShowFeatures);
            view.DestroyButton.onClick.AddListener(ShowDestroy);
            view.RepairButton.onClick.AddListener(ShowRepair);
        }

        private void Show(string buildingName, int buildingLevel, Action infoAction, Action featureAction, Action destroyAction, Action repairAction)
        {
            base.Show();

            view.NameText.text = $"{buildingName} (Level {buildingLevel})";
            _infoAction = infoAction;
            _featureAction = featureAction;
            _destroyAction = destroyAction;
            _repairAction = repairAction;
        }

        private void ShowInfo()
        {
            Hide();
            _infoAction?.Invoke();
        }
        private void ShowFeatures()
        {
            Hide();
            _featureAction?.Invoke();
        }
        private void ShowDestroy()
        {
            Hide();
            _destroyAction?.Invoke();
        }
        private void ShowRepair()
        {
            Hide();
            _repairAction?.Invoke();
        }

    }

}