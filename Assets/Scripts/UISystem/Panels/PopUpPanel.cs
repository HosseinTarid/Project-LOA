using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class PopUpPanel : UIBasePanel
    {
        static PopUpData popUpData = new PopUpData();

        [Space]
        [SerializeField]
        Image bgImage;
        [SerializeField]
        Image buttonImage;
        [SerializeField]
        TextMeshProUGUI text;
        [SerializeField]
        TextMeshProUGUI buttonText;
        [SerializeField]
        Sprite blueBG;
        [SerializeField]
        Sprite redBG;
        [SerializeField]
        Sprite blueBGButton;
        [SerializeField]
        Sprite redBGButton;

        public override void Enter(Action onEnterDoneCB = null)
        {
            base.Enter(onEnterDoneCB);

            bgImage.sprite = popUpData.isBGRed ? redBG : blueBG;
            buttonImage.sprite = popUpData.isBGRed ? redBGButton : blueBGButton;
            text.text = popUpData.context;
            buttonText.text = popUpData.buttonTitle;

        }

        public override void Exit(Action onExitDoneCB = null)
        {
            base.Exit(onExitDoneCB);
            popUpData.popUpCloseAction?.Invoke();
        }

        public void OnButtonClick()
        {
            popUpData.buttonAction?.Invoke();
        }

        public static void SetPopUpData(string context, string buttonTitle, bool isBgRed, Action buttonAction, Action popUpCloseAction)
        {
            popUpData.SetData(context, buttonTitle, isBgRed, buttonAction, popUpCloseAction);
        }
    }

    public class PopUpData
    {
        public string context;
        public bool isBGRed;
        public string buttonTitle;
        public Action buttonAction;
        public Action popUpCloseAction;

        public void SetData(string context, string buttonTitle, bool isBgRed, Action buttonAction, Action popUpCloseAction)
        {
            this.context = context;
            this.buttonTitle = buttonTitle;
            this.isBGRed = isBgRed;
            this.buttonAction = buttonAction;
            this.popUpCloseAction = popUpCloseAction;
        }
    }
}