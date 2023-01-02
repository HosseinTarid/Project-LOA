using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace UI
{
    public class BuildingActionPanel : UIBasePanel
    {
        [Space]
        [SerializeField]
        TextMeshProUGUI buildingNameText;

        public override void Enter(Action onEnterDoneCB = null)
        {
            base.Enter(onEnterDoneCB);
            var buildingData = LOAGameManager.Instance.playerProfile.activeLand.GetBuilding(LOAGameManager.Instance.buildingsManager.selectedBuildingId);
            buildingNameText.text = $"{buildingData.buildingInfo.type.title} (Level {buildingData.buildingInfo.level})";
        }

        public override void Exit(Action onExitDoneCB = null)
        {
            base.Exit(onExitDoneCB);
        }

        public void OnInfoClick()
        {
            LOAGameManager.Instance.uIManager.ChangePanel(PanelType.BuildingInfo);
        }

        public void OnFeatureClick()
        {
        }

        public void OnDestroyClick()
        {
            PopUpPanel.SetPopUpData("Do you want to deconstruct this building?", "Yes", true
                    , () =>
                    {
                        LOAGameManager.Instance.playerProfile.DeconstructBuilding(LOAGameManager.Instance.buildingsManager.selectedBuildingId);
                        LOAGameManager.Instance.uIManager.ChangePanel(PanelType.Main);
                    }
                    , null);
            LOAGameManager.Instance.uIManager.ChangePanel(PanelType.PopUp);
        }

        public void OnRepairClick()
        {
        }

        public void OnOrbitClick()
        {
            LOAGameManager.Instance.CameraController.SetCurrentState(CameraMovement.CameraState.Orbit);
        }
    }
}