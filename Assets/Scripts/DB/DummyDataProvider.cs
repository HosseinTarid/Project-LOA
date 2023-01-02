using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Services.API_Service.Game.User;

namespace PlayerDataBase
{
    public class DummyDataProvider : IDataProvider
    {
        const string Active_Land_ID_Key = "ActiveLandID";
        GameUserProfile.GameUserProfileData data;
        ConstantData constants;

        public int lumen { get => GetCurrencyAmount(CurrencyType.lumen); }
        public int platinum { get => GetCurrencyAmount(CurrencyType.platinum); }
        public int diamond { get => GetCurrencyAmount(CurrencyType.diamond); }
        public GameUserProfile.Land activeLand
        {
            get
            {
                int activeLandID = PlayerPrefs.GetInt(Active_Land_ID_Key);
                return data.lands.Find(x=>x.id == activeLandID);
            }
        }
        public List<GameUserProfile.Land> lands { get => data.lands; }
        public List<GameUserProfile.Building> availableBuildings { get => data.availableBuildings; }
        public ConstantData constantData => constants;

        public void FetchData(Action onDataFetched)
        {
            string text = Resources.Load<TextAsset>("DummyData/PlayerInfo").text;
            this.data = Newtonsoft.Json.JsonConvert.DeserializeObject<GameUserProfile.GameUserProfileData>(text);

            text = Resources.Load<TextAsset>("DummyData/ConstantData").text;
            constants = Newtonsoft.Json.JsonConvert.DeserializeObject<ConstantData>(text);

            SetActiveLand();

            onDataFetched?.Invoke();
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
            GameUserProfile.Building newBuilding = availableBuildings.Find(x => x.id == buildingId);
            newBuilding.position = new GameUserProfile.BuildingCoordinate();
            newBuilding.position.coordinate_x = pos.x;
            newBuilding.position.coordinate_y = pos.y;
            newBuilding.Status = BuildingStatus.constructing;
            newBuilding.construct_remaining_time = constantData.buildingsInfo.Find(x => x.buildingType == newBuilding.buildingInfo.type.type).info[newBuilding.buildingInfo.level].constructDuration;
            newBuilding.SetReceivedTime();
            activeLand.gameBuildings.Add(newBuilding);
            availableBuildings.RemoveAll(x => x.id == buildingId);
        }

        public void DeconstructBuilding(int buildingId)
        {
            var building = activeLand.GetBuilding(buildingId);
            building.Status = BuildingStatus.destructing;
            building.destruct_remaining_time = constantData.buildingsInfo.Find(x => x.buildingType == building.buildingInfo.type.type).info[building.buildingInfo.level].deconstructDuration;
            building.SetReceivedTime();
            LOAGameManager.Instance.buildingsManager.GetBuilding(buildingId).Init(buildingId);
        }

        public void OnBuildingDeconstructed(int buildingId)
        {
            LOAGameManager.Instance.buildingsManager.RemoveBuilding(buildingId);
            activeLand.gameBuildings.RemoveAll(x => x.id == buildingId);
        }

        public void OnFinishNow(int buildingId)
        {
            GameUserProfile.Building building = activeLand.GetBuilding(buildingId);
            building.construct_remaining_time = 0;
        }
    }
}
