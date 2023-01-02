using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Script.Services.API_Service.Game.User;

namespace UI
{
    public class LandPanelItem : MonoBehaviour
    {
        [SerializeField]
        Image landImage;
        [SerializeField]
        TextMeshProUGUI landNameText;

        Action onItemClickCB;
        GameUserProfile.Land data;
        public void SetData(GameUserProfile.Land data)
        {
            this.data = data;
            landNameText.text = data.categoryName;
        }

        public void OnItemClick()
        {
            onItemClickCB?.Invoke();

            LOAGameManager.Instance.LoadLand(data.id);
            /* bool result = BuildingSystem.BuildingsManager.Instance.CheckForPlacingNewBilding(BuildingSystem.BuildingType.Smaple);
             if (result)
                 LOAGameManager.Instance.uIManager.ChangePanel(PanelType.BuildingPlacement);
             else
                 Debug.LogError("Not enough Space for this building.");*/
        }
    }
}
