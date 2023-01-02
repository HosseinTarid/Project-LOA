using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerDataBase;
using UI;
using LOATerrain;
using BuildingSystem;
using CameraMovement;

public class LOAGameManager : Utility.Patterns.MonoBehaviourSingleton<LOAGameManager>
{
    [SerializeField]
    CameraController cameraController;
    public CameraController CameraController => cameraController;
    [SerializeField]
    GridManager gridManager;
    public GridManager GridManager => gridManager;
    public IDataProvider playerProfile { get; private set; }
    public UIManager uIManager { get; private set; }
    public TerrainController terrainController { get; private set; }
    public BuildingsManager buildingsManager;
    public GameAPIs gameAPIs;

    public bool isOffline { private set; get; }

    protected override void Awake()
    {
        base.Awake();

        uIManager = new UIManager();
        uIManager.Init();

        terrainController = new TerrainController();

        buildingsManager = new BuildingsManager();
        gameAPIs = new GameAPIs();

        gridManager.CreatGrid();
    }

    void Update()
    {
        uIManager?.Update();
    }

    public void OfflineLogIn()
    {
        isOffline = true;
        playerProfile = new PlayerDataBase.DummyDataProvider();
        playerProfile.FetchData(OnProfileDataFetched);
    }

    public void OnLogInFinished()
    {
        isOffline = false;
        playerProfile = new PlayerDataBase.MainDataProvider();
        playerProfile.FetchData(OnProfileDataFetched);
    }

    void OnProfileDataFetched()
    {
        LoadLand(playerProfile.activeLand.id);
    }

    public void LoadLand(int landID)
    {
        playerProfile.SetActiveLand(landID);
        terrainController.ActivateTerrain(playerProfile.lands.Find(x => x.id == landID).Type, LoadBuildings);
    }

    public void LoadBuildings()
    {
        buildingsManager.PlaceLandBuildings();
        LOAGameManager.Instance.uIManager.ChangePanel(PanelType.Main);
    }
}
