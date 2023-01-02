using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Services.API_Service;
using Assets.Script.Services.API_Service.Auth;
using Assets.Script.Services.API_Service.Game.User;
using Assets.Script.Services.API_Service.Game.Buildings;


public class GameAPIs
{
    public void LogIn(string email, string password, string walletAddress, Action onSuccessful, Action<string> onFail)
    {

        Meta.Login(email, password, walletAddress,
            new HttpRequestAction<Login.LoginData>()
            {
                OnFinishedSuccess = (request, Response) =>
                {
                    onSuccessful?.Invoke();
                },
                OnError = (s) =>
                {
                    onFail?.Invoke("Login::OnError");
                },
                OnTimedOut = (s) =>
                {
                    onFail?.Invoke("Login::OnTimedOut");
                },
                OnAborted = (request) =>
                {
                    onFail?.Invoke("Login::OnAborted");
                },
                OnConnectionTimedOut = (request) =>
                {
                    onFail?.Invoke("Login::OnConnectionTimedOut");
                },
                OnFinishedFail = (request) =>
                {
                    onFail?.Invoke("Login::OnFinishedFail");
                }
            });
    }

    public void GetPlayerProfile(Action<GameUserProfile.GameUserProfileData> onSuccessful, Action<string> onFail)
    {

        Meta.GameUserProfile(new HttpRequestAction<GameUserProfile.GameUserProfileData>()
        {
            OnFinishedSuccess = (request, data) =>
            {
                onSuccessful?.Invoke(data);
            },
            OnAborted = (request) => { onFail?.Invoke("GameUserProfile::OnAborted"); },
            OnConnectionTimedOut = (request) => { onFail?.Invoke("GameUserProfile::OnConnectionTimedOut"); },
            OnError = (request) => { onFail?.Invoke("GameUserProfile::OnError"); },
            OnFinishedFail = (request) => { onFail?.Invoke("GameUserProfile::OnFinishedFail"); },
            OnTimedOut = (request) => { onFail?.Invoke("GameUserProfile::OnTimedOut"); }
        });
    }

    public void ConstructBuilding(int buildingId, Vector2Int buildingPos, Action onSuccessful, Action<string> onFail)
    {
        Meta.BuildingConstruct(LOAGameManager.Instance.playerProfile.activeLand.id, buildingId, buildingPos.x.ToString(), buildingPos.y.ToString(), "0", new HttpRequestAction<Construct.ConstructData>()
        {
            OnFinishedSuccess = (request, data) => onSuccessful?.Invoke(),
            OnError = (s) => onFail?.Invoke("BuildingConstruct::OnError"),
            OnTimedOut = (s) => onFail?.Invoke("BuildingConstruct::OnTimedOut"),
            OnAborted = (request) => onFail?.Invoke("BuildingConstruct::OnAborted"),
            OnConnectionTimedOut = (request) => onFail?.Invoke("BuildingConstruct::OnConnectionTimedOut"),
            OnFinishedFail = (request) => onFail?.Invoke("BuildingConstruct::OnFinishedFail")
        });
    }

    public void DeconstructBuilding(int buildingId, Action onSuccessful, Action<string> onFail)
    {
        Debug.Log(buildingId);
        Meta.Destruct(LOAGameManager.Instance.playerProfile.activeLand.id, buildingId, new HttpRequestAction<Destruct.DestructData>()
        {
            OnFinishedSuccess = (request, data) => onSuccessful?.Invoke(),
            OnError = (s) => onFail?.Invoke("DeconstructBuilding::OnError"),
            OnTimedOut = (s) => onFail?.Invoke("DeconstructBuilding::OnTimedOut"),
            OnAborted = (request) => onFail?.Invoke("DeconstructBuilding::OnAborted"),
            OnConnectionTimedOut = (request) => onFail?.Invoke("DeconstructBuilding::OnConnectionTimedOut"),
            OnFinishedFail = (request) => onFail?.Invoke("DeconstructBuilding::OnFinishedFail")
        });
    }
}
