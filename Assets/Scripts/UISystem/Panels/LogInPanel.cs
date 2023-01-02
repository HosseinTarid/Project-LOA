using UnityEngine;
using TMPro;
using System;
using Assets.Script.Services.API_Service;
using Assets.Script.Services.API_Service.Auth;


namespace UI
{
    public class LogInPanel : UIBasePanel
    {
        [Space]
        [SerializeField]
        TMP_InputField emailInput;
        [SerializeField]
        TMP_InputField walletInput;
        [SerializeField]
        TMP_InputField passwordInput;
        [SerializeField]
        TMP_InputField urlInput;
        [SerializeField]
        TextMeshProUGUI warningText;
        bool isWaitingForResponse;
        public override void Enter(Action onEnterDoneCB = null)
        {
            base.Enter(onEnterDoneCB);

            emailInput.text = "eshahriari94@gmail.com";
            walletInput.text = "123456789123451678912340";
            passwordInput.text = "Ehsan1994";
        }

        public void OnOfflineClike()
        {
            DoOnMainThread.Instance.Do(() => { LOAGameManager.Instance.OfflineLogIn(); });
        }

        public void OnLogInClick()
        {
            if (isWaitingForResponse)
                return;

            isWaitingForResponse = true;
            warningText.text = "Wait for response...";
            LOAGameManager.Instance.gameAPIs.LogIn(emailInput.text, passwordInput.text, walletInput.text
                            , () =>
                            {
                                isWaitingForResponse = false;
                                DoOnMainThread.Instance.Do(() => { LOAGameManager.Instance.OnLogInFinished(); });
                                warningText.text = "Logged in";
                            }, (message) =>
                            {
                                Debug.LogError(message);
                                isWaitingForResponse = false;
                                warningText.text = "Error";
                            });
        }

        ////////////////////////////////////////

        [System.Serializable]
        class LogInData
        {
            public string email;
            public string walletAddress;
            public string password;
            public string url;

            public string GetDataSum()
            {
                return $"{email}, {walletAddress}, {url}";
            }
        }
    }
}


