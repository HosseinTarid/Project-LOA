using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using BestHTTP;
using UnityEngine;

namespace Assets.Script.Services.API_Service
{
    public class ApiConfig
    {
        public string URL;
        public HTTPMethods method;
        public Dictionary<string, string> fields;
        public Dictionary<string, string> Headers;
        public bool NeedAutorization;
        public bool NeedXapiKey;
    }

    public static class ApiServices
    {
        public const string BaseUrl = "https://dev.landsofazolite.com/api/";
        public static string _token = "";
        public static string _last_login = "";
        public static string _key = "";

        public static void SendApi<T>(ApiConfig config, HttpRequestAction<T> actions)
        {
            if (config.NeedAutorization)
            {
                if (config.Headers == null)
                    config.Headers = new Dictionary<string, string>();
                config.Headers.Add("Authorization", $"Bearer {_token}");

            }
            if (config.NeedXapiKey)
            {
                if (config.Headers == null)
                    config.Headers = new Dictionary<string, string>();
                config.Headers.Add("x-api-key", GenerateXapiKey());
            }
            HttpServices.HttpSend(BaseUrl + config.URL, config.method, config.fields, config.Headers, actions);
        }

  public static void SendAuthorizedApi<T>(string ServiceAddress, HTTPMethods method, Dictionary<string, string> fields,
            Dictionary<string, string> Headers, HttpRequestAction<T> actions)
        {
            if (Headers == null)
                Headers = new Dictionary<string, string>();
            Headers.Add("Authorization", $"Bearer {ApiServices._token}");
            HttpServices.HttpSend(BaseUrl + ServiceAddress, method, fields, Headers, actions);
        }

        private static string GenerateXapiKey()
        {
            string privateKey = "FSC0C5MLV9YUONLF5M3MGGAG4SOWFTLM";
            //$user->last_login + ':' + request()->bearerToken() + ':' + $privateKey->value + ':' + $user->key
            //Debug.Log($"_last_login={_last_login}");
            //Debug.Log($"_token={_token}");
            //Debug.Log($"privateKey={privateKey}");
            //Debug.Log($"_key={_key}");
            string xapiRaw = $"{_last_login}:{_token}:{privateKey}:{_key}";
            //Debug.Log($"xapiRaw={xapiRaw}");
            string md5=GetMD5(xapiRaw);
            //Debug.Log($"GetMD5={md5}");
            return md5;
        }

        private static string GetMD5(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

    }
}