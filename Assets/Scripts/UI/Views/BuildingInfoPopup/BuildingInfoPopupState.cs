using UnityEngine;
using UI.Core;
using System;

namespace UI.BuildingInfoPopup
{

    public class BuildingInfoPopupState : ScreenState<BuildingInfoPopupState, BuildingInfoPopupView>
    {

        public override void Initialize()
        {
            base.Initialize();
        }

        private void Show(Sprite buildingSpr, string buildingInfo, bool showStatistic, float firstStatisticValue = 0, float secondStatisticValue = 0, float thirdStatisticValue = 0)
        {
            base.Show();

            view.InfoPopupGO.SetActive(!showStatistic);
            view.StatisticPopupGO.SetActive(showStatistic);

            for (int i = 0; i < view.ItemImages.Length; i++)
                view.ItemImages[i].sprite = buildingSpr;

            for (int i = 0; i < view.InfoTexts.Length; i++)
                view.InfoTexts[i].text = buildingInfo;

            view.FirstStatisticBar.fillAmount = firstStatisticValue;
            view.SecondStatisticBar.fillAmount = secondStatisticValue;
            view.ThirdStatisticBar.fillAmount = thirdStatisticValue;
        }

    }

}