using System;
using System.Collections.Generic;
using Assets.Script.Services.API_Service.Login;
using BestHTTP;

namespace Assets.Script.Services.API_Service.Game.User
{
    public enum CurrencyType { lumen, platinum, diamond, USDT, ECG }
    public enum BuildingType { supplyDepot, reactor, strategyHall, commandCenter, miner, satellite, apartment, dropZone, researchCenter, bank, depthDetector }
    public enum BuildingStatus { ready, constructing, destructing }

    public class GameUserProfile : IApiCommands
    {
        public void Send<T>(HttpRequestAction<T> actions)
        {
            var config = new ApiConfig()
            {
                Headers = null,
                method = HTTPMethods.Get,
                fields = null,
                NeedAutorization = true,
                NeedXapiKey = true,
                URL = "game/users/profile"

            };

            ApiServices.SendApi(config, actions);
        }


        public class Building
        {
            public int id { get; set; }
            public BuildingInfo buildingInfo { get; set; }
            BuildingStatus status { get; set; }
            public BuildingStatus Status
            {
                get
                {
                    if (status == BuildingStatus.constructing && Construct_remaining_time == 0)
                        status = BuildingStatus.ready;

                    return status;
                }
                set => status = value;
            }
            public object functionality { get; set; }
            public int construct_remaining_time { get; set; }
            public int Construct_remaining_time
            {
                get
                {
                    int remaining = construct_remaining_time - (int)(DateTime.Now - receivedTime).TotalSeconds;
                    remaining = remaining < 0 ? 0 : remaining;

                    return remaining;
                }
            }
            public int destruct_remaining_time { get; set; }
            public int Destruct_remaining_time
            {
                get
                {
                    int remaining = destruct_remaining_time - (int)(DateTime.Now - receivedTime).TotalSeconds;
                    remaining = remaining < 0 ? 0 : remaining;
                    return remaining;
                }
            }
            public BuildingCoordinate position { get; set; }
            DateTime receivedTime;

            public void SetReceivedTime() => receivedTime = DateTime.Now;
        }

        public class BuildingInfo
        {
            public string name { get; set; }
            public string description { get; set; }
            public BuildingTypeInfo type { get; set; }
            public int level { get; set; }
            public string image { get; set; }
            public int power { get; set; }
            public int experience { get; set; }
            public int prestige { get; set; }
        }

        public class BuildingTypeInfo
        {
            public string title { get; set; }
            public BuildingType type { get; set; }
        }

        public class BuildingCoordinate
        {
            public int coordinate_x { get; set; }
            public int coordinate_y { get; set; }
        }

        public class Currency
        {
            public CurrencyType symbol { get; set; }
            public int amount { get; set; }
        }

        public class Force
        {
            public int version_code { get; set; }
            public string version { get; set; }
            public string type { get; set; }
            public string description { get; set; }
            public object link { get; set; }
            public DateTime created_at { get; set; }
        }

        public class GameVersion
        {
            public Update update { get; set; }
            public Force force { get; set; }
        }

        public class Land
        {
            public int id { get; set; }
            public string categoryName { get; set; }
            public string categoryKey { get; set; }
            public string landscape { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public int prestige { get; set; }
            public int power { get; set; }
            public List<Building> gameBuildings { get; set; }

            public LOATerrain.TerrainType Type
            {
                get
                {
                    if (categoryKey == "silent_moor")
                        return LOATerrain.TerrainType.Jungle;
                    else if (categoryKey == "beach")
                        return LOATerrain.TerrainType.Beach;
                    else if (categoryKey == "mountain")
                        return LOATerrain.TerrainType.Mountain;
                    else 
                        return LOATerrain.TerrainType.Desert;
                }
            }

            public Building GetBuilding(int buildingId) => gameBuildings.Find(x => x.id == buildingId);

        }

        public class GameUserProfileData
        {
            public string email { get; set; }
            public List<Land> lands { get; set; }
            public List<Building> availableBuildings { get; set; }
            public List<Currency> wallets { get; set; }
            public GameVersion gameVersion { get; set; }
            public List<object> messages { get; set; }
        }

        public class Update
        {
            public int version_code { get; set; }
            public string version { get; set; }
            public string type { get; set; }
            public string description { get; set; }
            public object link { get; set; }
            public DateTime created_at { get; set; }
        }
    }
}
