using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Assets.Script.Services.API_Service;
using Assets.Script.Services.API_Service.Auth;
using Assets.Script.Services.API_Service.Game.Buildings;
using Assets.Script.Services.API_Service.Game.Landing;
using Assets.Script.Services.API_Service.Game.User;
using Assets.Script.Services.API_Service.Login;
using Assets.Script.Services.API_Service.Profile;
using BestHTTP;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;



public class ApiServiceSamples : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Login()
    {
        // Meta.Login("saeedalipanah1374@gmail.com", "12345678", "TXS86na8FoaXjT87F4W3P6TCHHivHXjNK4",
        //Meta.Login("poorya.3258@gmail.com", "password", "12345678912345167891234x",
        Meta.Login("eshahriari94@gmail.com", "Ehsan1994", "123456789123451678912340",
            new HttpRequestAction<Login.LoginData>()
            {
                OnFinishedSuccess = (request, Response) =>
                {
                    Debug.Log(request.Response.DataAsText);
                },
                OnError = request =>
                {
                    Debug.Log("Error on Login");
                    Debug.Log(request.Response.Message);
                    Debug.Log(request.Response.DataAsText);
                }
            });
    }

    public void UserInfo()
    {
        Meta.UserProfile(new HttpRequestAction<UserProfile.Data>()
        {
            OnFinishedSuccess = (request, data) =>
           {
               Debug.Log(request.Response.DataAsText);
               Debug.Log(data.result.email);
           }
        });

    }

    public void GameUserProfile()
    {
        Meta.GameUserProfile(new HttpRequestAction<GameUserProfile.GameUserProfileData>()
        {
            OnFinishedSuccess = (request, data) =>
            {
                Debug.Log(request.Response.DataAsText);
                Debug.Log(data.email);

            }
        });

    }

    public void Regsiter()
    {
        Meta.Register("ahmad", "poorya.3258@gmail.com", "password", "password"
           , "12345678912345167891234x", new HttpRequestAction<Register.RegisterData>()
           {
               OnFinishedSuccess = (request, data) =>
               {
                   Debug.Log(request.Response.DataAsText);
                   Debug.Log(data.result.email);

               }
           });

    }

    public void Construct()
    {
        Meta.BuildingConstruct(1, 12, "15.658", "74.5874", "45.6589"
            , new HttpRequestAction<Construct.ConstructData>()
            {
                OnFinishedSuccess = (request, data) =>
                {
                    Debug.Log(request.Response.DataAsText);
                    Debug.Log(data.result.status);
                }
            });

    }


    public void Landing()
    {
        int landid = 1;
        Meta.Landing(landid, new HttpRequestAction<Landing.LandingData>()
        {
            OnFinishedSuccess = (request, data) =>
            {
                Debug.Log(request.Response.DataAsText);
                Debug.Log(data.data.name);

            }
        });

    }

    public void Destruct()
    {
        int landid = 1;
        int buildingId = 1;
        Meta.Destruct(landid, buildingId, new HttpRequestAction<Destruct.DestructData>()
        {
            OnFinishedSuccess = (request, data) =>
            {
                Debug.Log(request.Response.DataAsText);
                Debug.Log(data.result.status);

            }
        });

    }


    public void StartWorking()
    {
        Meta.StartWorking(6, new HttpRequestAction<StartWorking.StartWorkingData>()
        {
            OnFinishedSuccess = (request, data) =>
            {
                Debug.Log(request.Response.DataAsText);
                Debug.Log(data.result.capacity);
            }
        });

    } 
    
    public void BoostConstruct()
    {
        Meta.BoostConstruct(4, new HttpRequestAction<BoostConstruct.BoostConstructData>()
        {
            OnFinishedSuccess = (request, data) =>
            {
                Debug.Log(request.Response.DataAsText);
                Debug.Log(data.result.status);
            }
        });

    }
}