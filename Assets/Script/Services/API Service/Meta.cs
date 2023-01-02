


using Assets.Script.Services.API_Service.Auth;
using Assets.Script.Services.API_Service.Game.Buildings;
using Assets.Script.Services.API_Service.Game.Landing;
using Assets.Script.Services.API_Service.Game.User;
using UnityEngine;

namespace Assets.Script.Services.API_Service
{
    public static class Meta
    {
        public static void Login<T>(string email, string password, string wallet_address, HttpRequestAction<T> actions)
        {
            var login = new Auth.Login(email, password, wallet_address);
            actions.OnFinishedSuccess += (request, arg2) =>
            {
                ApiServices._token = ((arg2 as Auth.Login.LoginData).result.token);
                ApiServices._last_login = ((arg2 as Auth.Login.LoginData).result.lastLogin.ToString());
                ApiServices._key = ((arg2 as Auth.Login.LoginData).result.key);
            };
            login.Send(actions);
        }

        public static void Register<T>(string name, string email, string password, string passwordConfirmation,
            string wallet_address, HttpRequestAction<T> actions)
        {
            var register = new Register(name, email, password, passwordConfirmation, wallet_address);
            actions.OnFinishedSuccess += (request, arg2) =>
            {
                ApiServices._token = ((arg2 as Auth.Register.RegisterData).result.token);
            };
            register.Send(actions);
        }

        public static void UserProfile<T>(HttpRequestAction<T> actions)
        {
            var info = new Profile.UserProfile();
            info.Send(actions);
        }
        public static void GameUserProfile<T>(HttpRequestAction<T> actions)
        {
            var gameUserProfile = new GameUserProfile();
            gameUserProfile.Send(actions);
        }

        public static void BuildingConstruct<T>(int land_id, int buildingId, string coordinateX, string coordinateY, string coordinateZ, HttpRequestAction<T> actions)
        {
            var construct = new Construct(land_id, buildingId, coordinateX, coordinateY, coordinateZ);
            construct.Send(actions);
        }

        //public static void EditProfile<T>(int land_id, string coordinateX, string coordinateY, string coordinateZ,HttpRequestAction<T> actions)
        //{
        //    var construct = new Construct(land_id, coordinateX, coordinateY, coordinateZ);
        //    construct.Send(actions);
        //}

        public static void Landing<T>(int landId, HttpRequestAction<T> actions)
        {
            var land = new Landing(landId);
            land.Send(actions);
        }

        public static void Destruct<T>(int landId,int buildingId,HttpRequestAction<T> actions)
        {
            var destruct = new Destruct(landId, buildingId);
            destruct.Send(actions);
        }

        public static void StartWorking<T>(int buildingId, HttpRequestAction<T> actions)
        {
            var startWorking = new StartWorking(buildingId);
            startWorking.Send(actions);
        } 
        public static void BoostConstruct<T>(int buildingId, HttpRequestAction<T> actions)
        {
            var boostConstruct = new BoostConstruct(buildingId);
            boostConstruct.Send(actions);
        }
    }
}