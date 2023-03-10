using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Services.API_Service.Login;
using BestHTTP;

namespace Assets.Script.Services.API_Service.Game.Buildings
{
    public class StartWorking : IApiCommands
    {
        private int _BuildingId;

        public StartWorking(int buildingId)
        {
            _BuildingId = buildingId;
        }
        public void Send<T>(HttpRequestAction<T> actions)
        {
            var config = new ApiConfig()
            {
                Headers = null,
                method = HTTPMethods.Post,
                fields = null,
                NeedAutorization = true,
                NeedXapiKey = true,
                URL = $"game / buildings /{_BuildingId}/startWorking",
            };

            ApiServices.SendApi(config, actions);
        }



        public class BuildingInfo
        {
            public string name { get; set; }
            public Type type { get; set; }
            public string description { get; set; }
            public int level { get; set; }
            public string image { get; set; }
            public int power { get; set; }
            public int experience { get; set; }
            public int prestige { get; set; }
        }

        public class ConstructionCost
        {
            public string coin { get; set; }
            public int value { get; set; }
        }

        public class Info
        {
            public int id { get; set; }
            public int land_id { get; set; }
            public BuildingInfo buildingInfo { get; set; }
            public ConstructionCost construction_cost { get; set; }
            public string status { get; set; }
            public string construct_started_at { get; set; }
            public string construct_finished_at { get; set; }
            public object destruct_started_at { get; set; }
            public object destruct_finished_at { get; set; }
        }

        public class Result
        {
            public int id { get; set; }
            public int power { get; set; }
            public int capacity { get; set; }
            public int prestige { get; set; }
            public string status { get; set; }
            public Info info { get; set; }
            public object collected_at { get; set; }
            public object finished_at { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }

        public class StartWorkingData
        {
            public bool success { get; set; }
            public string message { get; set; }
            public Result result { get; set; }
            public List<object> errors { get; set; }
        }

        public class Type
        {
            public string title { get; set; }
            public string type { get; set; }
        }


    }
}
