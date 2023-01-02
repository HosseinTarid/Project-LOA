using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BuildingSystem;
using TMPro;


namespace UI
{
    public class BuildingPlacementPanel : UIBasePanel
    {
        [Space]
        [SerializeField]
        Button confirmButton;

        public override void Enter(Action onEnterDoneCB = null)
        {
            base.Enter(onEnterDoneCB);

            LOAGameManager.Instance.buildingsManager.StartPlacingCheckedBuilding(UpdateNewBuildingPlacingStatus);
        }

        void UpdateNewBuildingPlacingStatus(bool status)
        {
            confirmButton.interactable = status;
        }

        public void OnConfirmClick()
        {
            LOAGameManager.Instance.buildingsManager.ConfirmBuildingPlacing();
            LOAGameManager.Instance.uIManager.ChangePanel(PanelType.Main);
        }

        public void OnCancelClick()
        {
            LOAGameManager.Instance.buildingsManager.CancelBuildingPlacing();
            LOAGameManager.Instance.uIManager.ChangePanel(PanelType.Main);
        }
    }
}