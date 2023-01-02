using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class BuildingInfoPanelBar : MonoBehaviour
    {
        [SerializeField]
        Image iconImage;
        [SerializeField]
        TextMeshProUGUI titleText;
        [SerializeField]
        Image fillImage;

        public void SetData(Sprite icon, string title, float fillValue)
        {
            /*iconImage.sprite = icon;
            titleText.text = title;*/
            fillImage.fillAmount = Mathf.Clamp01(fillValue);
        }
    }
}