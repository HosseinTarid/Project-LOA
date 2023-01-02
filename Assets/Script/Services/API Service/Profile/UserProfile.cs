using System;
using System.Collections.Generic;
using Assets.Script.Services.API_Service.Login;
using BestHTTP;

namespace Assets.Script.Services.API_Service.Profile
{
    public class UserProfile : IApiCommands
    {
        //private Dictionary<string, string> _headers;

        public void Send<T>(HttpRequestAction<T> actions)
        {
            var config = new ApiConfig()
            {
                Headers =null ,
                method = HTTPMethods.Get,
                fields = null,
                NeedAutorization =true ,
                NeedXapiKey = false,
                URL = "users"
            };
            //ApiServices.SendAuthorizedApi("users", HTTPMethods.Get, null, _headers, actions);
            ApiServices.SendApi(config,actions);
        }

        public class Coin
        {
            public int coin_id { get; set; }
            public string coin_name { get; set; }
            public int amount { get; set; }
        }

        public class Result
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public object email_verified_at { get; set; }
            public string wallet_address { get; set; }
            public List<Coin> coins { get; set; }
            public List<Land> lands { get; set; }
        }

       

        public class Data
        {
            public bool success { get; set; }
            public string message { get; set; }
            public Result result { get; set; }
            public List<object> errors { get; set; }
        }






    }
}
