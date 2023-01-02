using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Services.API_Service;
using Assets.Script.Services.API_Service.Game.User;

namespace PlayerDataBase
{
    public class MainDataProvider : IDataProvider
    {
        const string Active_Land_ID_Key = "ActiveLandID";
        GameUserProfile.GameUserProfileData data;

        public int lumen { get => GetCurrencyAmount(CurrencyType.lumen); }
        public int platinum { get => GetCurrencyAmount(CurrencyType.platinum); }
        public int diamond { get => GetCurrencyAmount(CurrencyType.diamond); }
        public GameUserProfile.Land activeLand
        {
            get
            {
                int activeLandID = PlayerPrefs.GetInt(Active_Land_ID_Key);
                foreach (var land in data.lands)
                {
                    if (land.id == activeLandID)
                        return land;
                }

                return null;
            }
        }
        public List<GameUserProfile.Land> lands { get => data.lands; }
        public List<GameUserProfile.Building> availableBuildings { get => data.availableBuildings; }
        public ConstantData constantData => null;
        public void FetchData(Action onDataFetched)
        {
            LOAGameManager.Instance.gameAPIs.GetPlayerProfile(
                (data) =>
                {
                    this.data = data;
                    SetActiveLand();

                    onDataFetched?.Invoke();
                },
                (message) => Debug.Log(message));
        }

        int GetCurrencyAmount(CurrencyType type)
        {
            foreach (var currency in data.wallets)
            {
                if (currency.symbol == type)
                    return currency.amount;
            }

            return 0;
        }

        void SetActiveLand()
        {
            if (PlayerPrefs.GetInt(Active_Land_ID_Key, -1) == -1 && data.lands != null && data.lands.Count > 0)
                SetActiveLand(data.lands[0].id);
        }

        public void SetActiveLand(int id)
        {
            PlayerPrefs.SetInt(Active_Land_ID_Key, id);
        }

        public void ConstructBuilding(int buildingId, Vector2Int pos)
        {
            LOAGameManager.Instance.gameAPIs.ConstructBuilding(buildingId, pos, () =>
            {
                GameUserProfile.Building newBuilding = availableBuildings.Find(x => x.id == buildingId);
                newBuilding.position = new GameUserProfile.BuildingCoordinate();
                newBuilding.position.coordinate_x = pos.x;
                newBuilding.position.coordinate_y = pos.y;
                newBuilding.Status = BuildingStatus.constructing;
                //newBuilding.construct_remaining_time = constantData.buildingsInfo.Find(x => x.buildingType == newBuilding.buildingInfo.type.type).info[newBuilding.buildingInfo.level].constructDuration;
                activeLand.gameBuildings.Add(newBuilding);
                availableBuildings.RemoveAll(x => x.id == buildingId);
            }
            , null);
        }

        public void DeconstructBuilding(int buildingId)
        {
            LOAGameManager.Instance.gameAPIs.DeconstructBuilding(buildingId, () =>
            {
                var building = activeLand.GetBuilding(buildingId);
                building.Status = BuildingStatus.destructing;
                //building.destruct_remaining_time = constantData.buildingsInfo.Find(x => x.buildingType == building.buildingInfo.type.type).info[building.buildingInfo.level].deconstructDuration;
                building.SetReceivedTime();
            }
            , null);
        }

        public void OnBuildingDeconstructed(int buildingId)
        {
            LOAGameManager.Instance.buildingsManager.RemoveBuilding(buildingId);
            activeLand.gameBuildings.RemoveAll(x => x.id == buildingId);
            //TODO: see if it's needed to add it to availableBuildings or not.
        }

        public void OnFinishNow(int buildingId)
        {
            //GameUserProfile.Building building = activeLand.gameBuildings.Find(x => x.id == buildingId);
            //building.construct_remaining_time = 0;
        }
    }
}