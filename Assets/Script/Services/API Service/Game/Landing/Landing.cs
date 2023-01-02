using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Services.API_Service.Login;
using BestHTTP;

namespace Assets.Script.Services.API_Service.Game.Landing
{
    public class Landing : IApiCommands
    {
        private int _landId;
        public Landing(int Landid)
        {
            _landId = Landid;
        }
        public void Send<T>(HttpRequestAction<T> actions)
        {
            var config = new ApiConfig()
            {
                URL = $"game/landing/{_landId}",
                method = HTTPMethods.Get,
                fields = null,
                Headers = null,
                NeedAutorization = true,
                NeedXapiKey = true
            };
            ApiServices.SendApi(config, actions);
        }






        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Building
        {
            public int id { get; set; }
            public int building_id { get; set; }
            public object name { get; set; }
            public object image { get; set; }
            public object description { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }

        public class Data
        {
            public int id { get; set; }
            public int category_id { get; set; }
            public int landscape_id { get; set; }
            public int size_id { get; set; }
            public object map_info_id { get; set; }
            public string name { get; set; }
            public object flag { get; set; }
            public object art_portrait { get; set; }
            public object art_action { get; set; }
            public object description { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public List<object> gameBuildings { get; set; }
            public List<Building> buildings { get; set; }
        }

        public class LandingData
        {
            public Data data { get; set; }
        }








        /*
         {
    "data": {
        "id": 1,
        "category_id": 5,
        "landscape_id": 1,
        "size_id": 2,
        "map_info_id": null,
        "name": "REDKINDLES/Silent Moor(Field)",
        "flag": null,
        "art_portrait": null,
        "art_action": null,
        "description": null,
        "created_at": "2022-06-07T12:25:59.000000Z",
        "updated_at": "2022-06-07T12:25:59.000000Z",
        "gameBuildings": [],
        "buildings": [
            {
                "id": 4,
                "building_id": 1,
                "name": null,
                "image": null,
                "description": null,
                "created_at": "2022-07-26T11:38:39.000000Z",
                "updated_at": "2022-07-31T05:26:00.000000Z"
            },
            {
                "id": 6,
                "building_id": 6,
                "name": null,
                "image": null,
                "description": null,
                "created_at": "2022-07-26T11:38:39.000000Z",
                "updated_at": "2022-07-31T07:01:22.000000Z"
            },
            {
                "id": 7,
                "building_id": 1,
                "name": null,
                "image": null,
                "description": null,
                "created_at": "2022-07-26T11:38:39.000000Z",
                "updated_at": "2022-07-31T05:26:00.000000Z"
            },
            {
                "id": 8,
                "building_id": 6,
                "name": null,
                "image": null,
                "description": null,
                "created_at": "2022-07-26T11:38:39.000000Z",
                "updated_at": "2022-07-31T07:01:22.000000Z"
            },
            {
                "id": 9,
                "building_id": 1,
                "name": null,
                "image": null,
                "description": null,
                "created_at": "2022-07-26T11:38:39.000000Z",
                "updated_at": "2022-07-31T05:26:00.000000Z"
            },
            {
                "id": 10,
                "building_id": 6,
                "name": null,
                "image": null,
                "description": null,
                "created_at": "2022-07-26T11:38:39.000000Z",
                "updated_at": "2022-07-31T07:01:22.000000Z"
            },
            {
                "id": 11,
                "building_id": 1,
                "name": null,
                "image": null,
                "description": null,
                "created_at": "2022-07-26T11:38:39.000000Z",
                "updated_at": "2022-07-31T05:26:00.000000Z"
            },
            {
                "id": 12,
                "building_id": 6,
                "name": null,
                "image": null,
                "description": null,
                "created_at": "2022-07-26T11:38:39.000000Z",
                "updated_at": "2022-07-31T07:01:22.000000Z"
            }
        ]
    }
}
         */

    }
}
