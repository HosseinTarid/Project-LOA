using System;
using System.Collections.Generic;
using Assets.Script.Services.API_Service.Game.User;

namespace PlayerDataBase
{
    public class PlayerProfile
    {
        IDataProvider dataProvidr;

        public int Lumen { get => dataProvidr.lumen; }
        public int Platinum { get => dataProvidr.platinum; }
        public int Diamond { get => dataProvidr.diamond; }
        public GameUserProfile.Land activeLand { get => dataProvidr.activeLand; }
        public List<GameUserProfile.Land> lands { get => dataProvidr.lands; }
        public List<GameUserProfile.Building> availableBuildings { get => dataProvidr.availableBuildings; }

        public void Init(IDataProvider dataProvider, Action onDataFetched)
        {
            this.dataProvidr = dataProvider;
            this.dataProvidr.FetchData(onDataFetched);
        }

        public void SetActiveLand(int id)
        {
            dataProvidr.SetActiveLand(id);
        }
    }
}