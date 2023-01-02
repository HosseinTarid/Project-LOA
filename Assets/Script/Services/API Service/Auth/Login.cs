using System;
using System.Collections.Generic;
using Assets.Script.Services.API_Service.Login;
using BestHTTP;
using UnityEngine;

namespace Assets.Script.Services.API_Service.Auth
{
    public class Login : IApiCommands
    {
        private Dictionary<string, string> _parameters;
        public Login(string email, string password, string wallet_address)
        {
            _parameters = new Dictionary<string, string>();
            _parameters.Add("email", email);
            _parameters.Add("password", password);
            _parameters.Add("wallet_address", wallet_address);
        }

        public void Send<T>(HttpRequestAction<T> actions)
        {
            var config = new ApiConfig()
            {
                URL = "users/auth/login",
                method = HTTPMethods.Post,
                fields = _parameters,
                Headers = null
            };
            ApiServices.SendApi(config,actions);
        }

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

        public class Land
        {
            public int id { get; set; }
            public int user_id { get; set; }
            public int category_id { get; set; }
            public int landscape_id { get; set; }
            public int size_id { get; set; }
            public object map_info_id { get; set; }
            public string name { get; set; }
            public object flag { get; set; }
            public string status { get; set; }
            public string base_price { get; set; }
            public object art_portrait { get; set; }
            public object art_action { get; set; }
            public object description { get; set; }
            public DateTime? created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string price_description { get; set; }
            public object deleted_at { get; set; }
        }

        public class Result
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string lastLogin { get; set; }
            public string key { get; set; }
            public object email_verified_at { get; set; }
            public object wallet_address { get; set; }
            public string token { get; set; }
            public List<Coin> coins { get; set; }
            public List<Land> lands { get; set; }
        }

        public class LoginData
        {
            public bool success { get; set; }
            public object message { get; set; }
            public Result result { get; set; }
            public List<object> errors { get; set; }
        }



    }



}


public class Land
{
    public int id { get; set; }
    public int user_id { get; set; }
    public int category_id { get; set; }
    public int landscape_id { get; set; }
    public int size_id { get; set; }
    public object map_info_id { get; set; }
    public string name { get; set; }
    public object flag { get; set; }
    public string status { get; set; }
    public string base_price { get; set; }
    public object art_portrait { get; set; }
    public object art_action { get; set; }
    public object description { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string price_description { get; set; }
    public object deleted_at { get; set; }
}
