using UnityEngine.Networking;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using System;

namespace AHEFramework.Service.HTTP
{
    public enum HTTPRequestMethod { GET, POST, PUT }

    public class HTTPService : Utility.Patterns.MonoBehaviourSingleton<HTTPService>
    {
        public static void SendData(HTTPRequestMethod method, string url, Action<UnityWebRequest> callback = null, KeyValuePair<string, string>[] headers = default, string body = "") =>
            Instance.StartCoroutine(SendDataCoroutine(method, url, callback, headers, body));


        private static IEnumerator SendDataCoroutine(HTTPRequestMethod method, string url, Action<UnityWebRequest> callback, KeyValuePair<string, string>[] headers, string body)
        {
            UnityWebRequest request = new UnityWebRequest();
            request.method = method.ToString();
            request.url = url;

            // Add Body
            if (!string.IsNullOrEmpty(body))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
            request.downloadHandler = new DownloadHandlerBuffer();

            // Add Headers
            request.SetRequestHeader("Content-Type", "application/json");
            if (headers != default)
                foreach (var item in headers)
                    request.SetRequestHeader(item.Key, item.Value);

            yield return request.SendWebRequest();

            callback?.Invoke(request);
        }
    }
}
