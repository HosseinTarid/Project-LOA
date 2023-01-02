using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Services.API_Service.Login;
using BestHTTP;

namespace Assets.Script.Services.API_Service.Game.Buildings
{
    public class Destruct : IApiCommands
    {
        private int _landId;
        private int _buildingId;
        private Dictionary<string, string> _parameters;
        public Destruct(int Landid, int building)
        {
            _landId = Landid;
            _buildingId = building;
            _parameters = new Dictionary<string, string>();
            _parameters.Add("land_id", _landId.ToString());
        }

        public void Send<T>(HttpRequestAction<T> actions)
        {
            var config = new ApiConfig()
            {
                URL = $"game/buildings/{_buildingId}/destruct",
                method = HTTPMethods.Post,
                fields = _parameters,
                Headers = null,
                NeedAutorization = true,
                NeedXapiKey = true
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

        public class Result
        {
            public int id { get; set; }
            public int land_id { get; set; }
            public BuildingInfo buildingInfo { get; set; }
            public ConstructionCost construction_cost { get; set; }
            public string status { get; set; }
            public string construct_started_at { get; set; }
            public string construct_finished_at { get; set; }
            public DateTime destruct_started_at { get; set; }
            public DateTime destruct_finished_at { get; set; }
        }

        public class DestructData
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
