using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace UI
{
    public class BuildingInfoPanel : UIBasePanel
    {
        [Space]
        [SerializeField]
        TextMeshProUGUI nameText;
        [SerializeField]
        TextMeshProUGUI descriptionText;
        [SerializeField]
        Image image;
        [SerializeField]
        Pool<BuildingInfoPanelBar> barPool = new Pool<BuildingInfoPanelBar>();

        List<BuildingInfoPanelBar> bars = new List<BuildingInfoPanelBar>();

        public override void Enter(Action onEnterDoneCB = null)
        {
            base.Enter(onEnterDoneCB);

            /*nameText.text = "";
            descriptionText.text = "";
            image.sprite = null;
            */
            DisableBars();
            if (/*Has bars*/true)
            {
                barPool.Parent.gameObject.SetActive(true);
                for (int i = 0; i < 3; i++)
                {
                    BuildingInfoPanelBar newBar = barPool.GetActive;
                    newBar.SetData(null, "", 0.2f + 0.2f * i);
                    bars.Add(newBar);
                }
            }
            else
            {
                barPool.Parent.gameObject.SetActive(false);
            }
        }

        void DisableBars()
        {
            foreach (var bar in bars)
            {
                bar.gameObject.SetActive(false);
            }

            bars.Clear();
        }
    }
}