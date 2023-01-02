using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Services.API_Service.Login;
using BestHTTP;

namespace Assets.Script.Services.API_Service.Auth
{
    public class Register : IApiCommands
    {
        private Dictionary<string, string> _parameters;

        public Register(string name, string email, string password, string password_confirmation, string wallet_address)
        {
            _parameters = new Dictionary<string, string>();
            _parameters.Add("name", name);
            _parameters.Add("email", email);
            _parameters.Add("password", password);
            _parameters.Add("password_confirmation", password_confirmation);
            _parameters.Add("wallet_address", wallet_address);
        }

        public void Send<T>(HttpRequestAction<T> actions)
        {
            var config = new ApiConfig()
            {
                Headers = null,
                NeedXapiKey = false,
                NeedAutorization = false,
                URL = "users/auth/register",
                fields = _parameters,
                method = HTTPMethods.Post
            };
            ApiServices.SendApi(config, actions);
        }

        /*
         *     "name":"ahmad",
    "wallet_address":"12345678912345167891234x",
    "email": "poorya.3258@gmail.com",
    "password": "password",
    "password_confirmation": "password"
         */

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Coin
        {
            public int id { get; set; }
            public int user_id { get; set; }
            public int coin_id { get; set; }
            public int amount { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }

        public class Result
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string lastLogin { get; set; }
            public object key { get; set; }
            public object email_verified_at { get; set; }
            public object wallet_address { get; set; }
            public string token { get; set; }
            public List<Coin> coins { get; set; }
            public List<object> lands { get; set; }
        }

        public class RegisterData
        {
            public bool success { get; set; }
            public string message { get; set; }
            public Result result { get; set; }
            public List<object> errors { get; set; }
        }
    }
}
