using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Services.API_Service;
using Assets.Script.Services.API_Service.Game.Buildings;
using Assets.Script.Services.API_Service.Game.User;

namespace BuildingSystem
{
    public class BuildingsManager
    {
        BuildingPool buildingPool = new BuildingPool();
        List<BuildingBase> buildings = new List<BuildingBase>();

        BuildingBase newBuilding;
        Action<bool> newBuildingUpdateUIAction;
        Vector2Int newBuildingStartIndex;

        static BuildingUI buildingUI;
        public static BuildingUI BuildingUIPrefab
        {
            get
            {
                if (buildingUI == null)
                    buildingUI = Resources.Load<BuildingUI>("UI/BuildingUI/BuildingUI");
                return buildingUI;
            }
        }

        public bool IsDraggingBuilding
        {
            get
            {
                if (newBuilding != null && newBuilding.IsDragging)
                    return true;

                foreach (var building in buildings)
                    if (building.IsDragging)
                        return true;

                return false;
            }
        }

        public bool IsInDraggingMode => newBuilding != null;

        BuildingType newBuildingType;
        Vector3 firstCellPosition;
        int newBuildingId;
        public int selectedBuildingId { private set; get; }
        public event Action<Vector2> OnBuildingSelectedEvent;

        #region OnLandLoaded
        public void PlaceLandBuildings()
        {
            RemmoveLandBuildings();

            foreach (var building in LOAGameManager.Instance.playerProfile.activeLand.gameBuildings)
            {
                var temp = buildingPool.GetActive(building.buildingInfo.type.type, building.buildingInfo.level);
                temp.Init(building.id);
                Vector2Int tempPos = new Vector2Int(building.position.coordinate_x, building.position.coordinate_y);
                Vector3 buildingPosition = LOAGameManager.Instance.GridManager.GetCellPosition(tempPos) + new Vector3((temp.Size.x / 2f) * LOAGameManager.Instance.GridManager.CellSize,
                                                                                                                        0,
                                                                                                                        (temp.Size.y / 2f) * LOAGameManager.Instance.GridManager.CellSize);
                temp.transform.position = buildingPosition;
                bool result = LOAGameManager.Instance.GridManager.OccupySpace(new Vector2Int(building.position.coordinate_x, building.position.coordinate_y), temp.Size);
                if (!result)
                    Debug.LogError("Building position is WRONG!!");
                buildings.Add(temp);
            }
        }

        void RemmoveLandBuildings()
        {
            buildings.ForEach(x => x.gameObject.SetActive(false));
            buildings.Clear();
            LOAGameManager.Instance.GridManager.ClearGrid();
        }
        #endregion

        #region Constructing Building
        public bool CheckForPlacingNewBilding(BuildingType buildingType, int level, int id)
        {
            if (newBuilding != null)
                return false;

            newBuilding = buildingPool.GetActive(buildingType, level);
            newBuilding.gameObject.SetActive(false);
            if (LOAGameManager.Instance.GridManager.GetFirstEmptySpace(newBuilding.Size, out newBuildingStartIndex, out firstCellPosition))
            {
                newBuildingId = id;
                newBuildingType = buildingType;
                return true;
            }
            else
            {
                newBuilding = null;
                return false;
            }
        }

        public void StartPlacingCheckedBuilding(Action<bool> UpdateUIAction)
        {
            if (newBuilding == null)
                return;

            newBuilding.gameObject.SetActive(true);
            newBuildingUpdateUIAction = UpdateUIAction;
            Vector3 buildingPosition = firstCellPosition + new Vector3((newBuilding.Size.x / 2f) * LOAGameManager.Instance.GridManager.CellSize,
                                                                           0,
                                                                           (newBuilding.Size.y / 2f) * LOAGameManager.Instance.GridManager.CellSize);
            newBuilding.transform.position = buildingPosition;

            Vector2 buildingPositionV2 = new Vector2(buildingPosition.x, buildingPosition.z);
            OnBuildingSelectedEvent?.Invoke(buildingPositionV2);

            newBuilding.isPlacing = true;
            bool canBUild = LOAGameManager.Instance.GridManager.CheckCellsAndSetColor(newBuildingStartIndex, newBuilding.Size);
            newBuildingUpdateUIAction?.Invoke(canBUild);
        }

        public bool OnBuildingPlacing(Vector3 position, Vector2Int buildingSize)
        {
            if (newBuilding == null)
                return false;

            newBuildingStartIndex = LOAGameManager.Instance.GridManager.GetNearestCellIndex(position, buildingSize);

            newBuilding.transform.position = new Vector3(LOAGameManager.Instance.GridManager.transform.position.x + newBuildingStartIndex.x * LOAGameManager.Instance.GridManager.CellSize + ((buildingSize.x / 2f) * LOAGameManager.Instance.GridManager.CellSize),
                                                        newBuilding.transform.position.y,
                                                        LOAGameManager.Instance.GridManager.transform.position.z + newBuildingStartIndex.y * LOAGameManager.Instance.GridManager.CellSize + ((buildingSize.y / 2f) * LOAGameManager.Instance.GridManager.CellSize));

            bool canBUild = LOAGameManager.Instance.GridManager.CheckCellsAndSetColor(newBuildingStartIndex, newBuilding.Size);
            newBuildingUpdateUIAction?.Invoke(canBUild);
            return true;
        }

        public void CancelBuildingPlacing()
        {
            if (newBuilding == null)
                return;

            LOAGameManager.Instance.GridManager.ResetCellsColor();
            newBuilding.gameObject.SetActive(false);
            newBuilding = null;
        }

        public void ConfirmBuildingPlacing()
        {
            LOAGameManager.Instance.GridManager.ResetCellsColor();
            LOAGameManager.Instance.GridManager.OccupySpace(newBuildingStartIndex, newBuilding.Size);
            newBuilding.isPlacing = false;
            buildings.Add(newBuilding);

            LOAGameManager.Instance.playerProfile.ConstructBuilding(newBuildingId, newBuildingStartIndex);
            newBuilding.Init(newBuildingId);
            newBuilding = null;
        }
        #endregion

        public void OnSelectBuilding(int buildingId)
        {
            selectedBuildingId = buildingId;

            var building = buildings.Find(x => x.BuildingID == buildingId);

            Vector2 buildingPosition = new Vector2(building.transform.position.x, building.transform.position.z);
            OnBuildingSelectedEvent?.Invoke(buildingPosition);
        }

        public void RemoveBuilding(int buildingId)
        {
            var building = buildings.Find(x => x.BuildingID == buildingId);
            var buildingInfo = LOAGameManager.Instance.playerProfile.activeLand.GetBuilding(buildingId);
            Vector2Int startIndex = new Vector2Int(buildingInfo.position.coordinate_x, buildingInfo.position.coordinate_y);
            LOAGameManager.Instance.GridManager.ClearSpace(startIndex, building.Size);
            building.gameObject.SetActive(false);
        }

        public BuildingBase GetBuilding(int buildingId) => buildings.Find(x => x.BuildingID == buildingId);
    }
}
