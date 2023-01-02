using UnityEngine;
using UnityWebRequest = UnityEngine.Networking.UnityWebRequest;
using IEnumerator = System.Collections.IEnumerator;
using System;

namespace AHEFramework.Service.HTTP
{
    public static class CheckConnection
    {
        /*public static void Check(Action<bool> resultAction) =>  // <HTD> Uncomment it later
            GlobalCoroutine.Run(Checking(resultAction));*/

        // For Internet Check
        private static IEnumerator Checking(Action<bool> action)
        {
            UnityWebRequest request = new UnityWebRequest("http://google.com");
            yield return request.SendWebRequest();
            if (request.error != null) // Connection Error
            {
#if UNITY_EDITOR
                Debug.LogError("Internet Connection ERROR: "+request.error);
#endif
                // Show PopUp Message Internet Connection Error
                action(false);
            }
            else
                action(true);
        }
    }
}