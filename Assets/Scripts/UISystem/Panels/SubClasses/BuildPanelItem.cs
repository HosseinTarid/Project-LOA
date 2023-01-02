using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Script.Services.API_Service.Game.User;
using Newtonsoft.Json;
using Newtonsoft;


namespace UI
{
    public class BuildPanelItem : MonoBehaviour
    {
        [SerializeField]
        Image buildingImage;
        [SerializeField]
        TextMeshProUGUI buildingNameText;
        [SerializeField]
        TextMeshProUGUI buildingLevelText;
        [SerializeField]
        TextMeshProUGUI buildingConstructionDurationText;
        [SerializeField]
        TextMeshProUGUI buildingIDKText;//count

        Action onItemClickCB;
        public void SetData(BuildingType type, int level, int count, Action onItemClick)
        {
            onItemClickCB = onItemClick;

            buildingNameText.text = type.ToString();
            buildingLevelText.text = level.ToString();
            buildingIDKText.text = count.ToString();
            
            if (LOAGameManager.Instance.isOffline)
                buildingConstructionDurationText.text = LOAGameManager.Instance.playerProfile.constantData.buildingsInfo.Find(x => x.buildingType == type).info[level].constructDuration.ToString();

        }

        public void OnItemClick()
        {
            onItemClickCB?.Invoke();
        }
    }
}
