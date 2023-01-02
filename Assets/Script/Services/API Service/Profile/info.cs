using System.Collections.Generic;
using Assets.Script.Services.API_Service.Login;
using BestHTTP;

namespace Assets.Script.Services.API_Service.Profile
{
    public class info : IApiCommands
    {
        private Dictionary<string, string> _headers;
        public info()
        {
  
        }

        public void Send<T>(HttpRequestAction<T> actions)
        {
            ApiServices.SendAuthorizedApi("users", HTTPMethods.Get, null, _headers, actions);
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
            public List<object> lands { get; set; }
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
