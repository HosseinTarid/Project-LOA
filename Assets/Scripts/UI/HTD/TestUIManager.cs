using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BuildingSystem;

public class TestUIManager : Utility.Patterns.MonoBehaviourSingleton<TestUIManager>
{
    [SerializeField]
    Canvas buildPanel;
    [SerializeField]
    Canvas confirmBuildPanel;
    [SerializeField]
    Canvas backToRTSCameraPanel;

    [SerializeField]
    Button confirmButton;
    [SerializeField]
    Button cancelButton;

    protected override void Awake()
    {
        base.Awake();

        OnBackToRTSCameraClick();

    }

    public void OnBuildClick()
    {
        /*bool result = BuildingsManager.Instance.StartPlacingBuilding(BuildingSystem.BuildingType.Smaple, UpdateNewBuildingPlacingStatus);

        if (result)
        {
            buildPanel.enabled = false;
            confirmBuildPanel.enabled = true;
        }*/
    }

    void UpdateNewBuildingPlacingStatus(bool status)
    {
        confirmButton.interactable = status;
    }

    public void OnConfirmClick()
    {
        LOAGameManager.Instance.buildingsManager.ConfirmBuildingPlacing();
        buildPanel.enabled = true;
        confirmBuildPanel.enabled = false;
        backToRTSCameraPanel.enabled = false;
    }

    public void OnCancelClick()
    {
        LOAGameManager.Instance.buildingsManager.CancelBuildingPlacing();
        buildPanel.enabled = true;
        confirmBuildPanel.enabled = false;
        backToRTSCameraPanel.enabled = false;
    }

    public void OnBackToRTSCameraClick()
    {
        buildPanel.enabled = true;
        confirmBuildPanel.enabled = false;
        backToRTSCameraPanel.enabled = false;
    }

    public void OnOrbitCameraClick()
    {
        buildPanel.enabled = false;
        confirmBuildPanel.enabled = false;
        backToRTSCameraPanel.enabled = true;
    }

    public void OnBackClick()
    {
        LOAGameManager.Instance.CameraController.SetCurrentState(CameraMovement.CameraState.RTS);
    }
}
