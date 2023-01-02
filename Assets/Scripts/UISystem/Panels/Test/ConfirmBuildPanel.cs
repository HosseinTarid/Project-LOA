using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI;
using System;
using BuildingSystem;

public class ConfirmBuildPanel : UIBasePanel
{
    [Space]
    [SerializeField]
    Button confirmButton;

    public override void Enter(Action onEnterDoneCB = null)
    {
        base.Enter(onEnterDoneCB);

        //bool result = BuildingsManager.Instance.StartPlacingBuilding(BuildingSystem.BuildingType.Smaple, UpdateNewBuildingPlacingStatus);
    }
    
    void UpdateNewBuildingPlacingStatus(bool status)
    {
        confirmButton.interactable = status;
    }

    public void OnConfirmClick()
    {
        LOAGameManager.Instance.buildingsManager.ConfirmBuildingPlacing();
        LOAGameManager.Instance.uIManager.ChangePanel(PanelType.Build);
    }

    public void OnCancelClick()
    {
        LOAGameManager.Instance.buildingsManager.CancelBuildingPlacing();
        LOAGameManager.Instance.uIManager.ChangePanel(PanelType.Build);
    }
}
