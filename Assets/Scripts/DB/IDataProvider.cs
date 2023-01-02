using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Script.Services.API_Service.Game.User;

namespace PlayerDataBase
{
    public interface IDataProvider
    {
        public int lumen { get; }
        public int platinum { get; }
        public int diamond { get; }
        public GameUserProfile.Land activeLand { get; }
        public List<GameUserProfile.Land> lands { get; }
        public List<GameUserProfile.Building> availableBuildings { get; }
        public ConstantData constantData { get; }
        public void FetchData(Action onDataFetched);
        public void SetActiveLand(int id);
        public void ConstructBuilding(int buildingId, Vector2Int pos);
        public void DeconstructBuilding(int buildingId);
        public void OnBuildingDeconstructed(int buildingId);
        public void OnFinishNow(int buildingId);
    }

    public class ConstantData
    {
        public List<BuildingInfo> buildingsInfo;
    }

    public class BuildingInfo
    {
        public BuildingType buildingType;
        public int width;
        public int height;
        public Dictionary<int, BuildingLevelInfo> info;
    }
    public class BuildingLevelInfo
    {
        public int cost;
        public int constructDuration;
        public int experience;
        public int power;
        public int prestige;
        public int deconstructDuration;
        public float deconstructCostPercentage;
    }
}