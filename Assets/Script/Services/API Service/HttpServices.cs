using System;
using System.Collections.Generic;
using BestHTTP;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Script.Services.API_Service
{
    public static class HttpServices
    {
        public static async void HttpSend<T>(string url, HTTPMethods method, Dictionary<string, string> fields,
            Dictionary<string, string> Headers, HttpRequestAction<T> actions)
        {
            Debug.Log($"url={url}  method={method.ToString()}");
            HTTPRequest request = new HTTPRequest(new Uri(url), method);

            if (fields != null)
                foreach (var field in fields)
                {
                    request.AddField(field.Key, field.Value);
                }

            if (Headers != null)
                foreach (var field in Headers)
                {
                    request.SetHeader(field.Key, field.Value);
                }
            request.Send();

            while (request.State < HTTPRequestStates.Finished)
            {
                //yield return new WaitForSeconds(0.1f);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f), ignoreTimeScale: false);
                string logtext = "";
                switch (request.State)
                {
                    // The request finished without any problem.
                    case HTTPRequestStates.Finished:

                        if (request.Response.IsSuccess)
                        {
                            Debug.Log(request.Response.DataAsText);
                            T DeserializedClass = JsonConvert.DeserializeObject<T>(request.Response.DataAsText);
                            actions.OnFinishedSuccess?.Invoke(request, DeserializedClass);
                            //Debug.Log(request.Response.DataAsText);
                        }
                        else
                        {
                            logtext = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                request.Response.StatusCode,
                                request.Response.Message,
                                request.Response.DataAsText);
                            Debug.LogWarning(logtext);
                            actions.OnFinishedFail?.Invoke(request);
                        }

                        break;

                    // The request finished with an unexpected error. The request's Exception property may contain more UserProfile about the error.
                    case HTTPRequestStates.Error:
                        logtext = "Request Finished with Error! " + (request.Exception != null ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "No Exception");
                        Debug.LogError(logtext);
                        actions.OnError?.Invoke(request);
                        break;

                    // The request aborted, initiated by the user.
                    case HTTPRequestStates.Aborted:
                        logtext = "Request Aborted!";
                        Debug.LogWarning(logtext);
                        actions.OnAborted?.Invoke(request);
                        break;

                    // Connecting to the server is timed out.
                    case HTTPRequestStates.ConnectionTimedOut:
                        logtext = "Connection Timed Out!";
                        Debug.LogError(logtext);
                        actions.OnConnectionTimedOut?.Invoke(request);
                        break;

                    // The request didn't finished in the given time.
                    case HTTPRequestStates.TimedOut:
                        logtext = "Processing the request Timed Out!";
                        Debug.LogError(logtext);
                        actions.OnTimedOut?.Invoke(request);
                        break;
                }

            }

        }
    }
}