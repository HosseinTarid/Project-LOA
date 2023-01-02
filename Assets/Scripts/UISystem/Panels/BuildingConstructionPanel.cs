using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Assets.Script.Services.API_Service.Game.User;

namespace UI
{
    public class BuildingConstructionPanel : UIBasePanel
    {
        [Space]
        [SerializeField]
        TextMeshProUGUI buildingNameText;
        [SerializeField]
        TextMeshProUGUI amountText;
        [SerializeField]
        TextMeshProUGUI progressText;
        [SerializeField]
        Image progressFillImage;

        public override void Enter(Action onEnterDoneCB = null)
        {
            base.Enter(onEnterDoneCB);

            int selectedBuildingId = LOAGameManager.Instance.buildingsManager.selectedBuildingId;
            var building = LOAGameManager.Instance.playerProfile.activeLand.GetBuilding(selectedBuildingId);

            buildingNameText.text = $"{building.buildingInfo.type.title} (Level {building.buildingInfo.level})";
            StartCoroutine(ShowConstructProgress(building));
        }

        public void OnFinishNowClick()
        {
            LOAGameManager.Instance.playerProfile.OnFinishNow(LOAGameManager.Instance.buildingsManager.selectedBuildingId);
        }

        IEnumerator ShowConstructProgress(GameUserProfile.Building building)
        {
            int constructDuration = LOAGameManager.Instance.playerProfile.constantData.buildingsInfo.Find(x => x.buildingType == building.buildingInfo.type.type).info[building.buildingInfo.level].constructDuration;
            float progress = 0;
            while (building.Construct_remaining_time > 0)
            {
                progress = (constructDuration - building.Construct_remaining_time) / (float)constructDuration;
                progressFillImage.fillAmount = progress;
                progressText.text = (Mathf.RoundToInt(progress * 100)) + "%";
                amountText.text = "0";
                yield return new WaitForSeconds(1);
            }
        }
    }
}