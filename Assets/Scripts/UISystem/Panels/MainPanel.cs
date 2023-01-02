using UnityEngine;
using TMPro;
using System;

namespace UI
{
    public class MainPanel : UIBasePanel
    {
        [Space]
        [SerializeField]
        TextMeshProUGUI lumen_Text;
        [SerializeField]
        TextMeshProUGUI platinum_Text;

        public override void Enter(Action onEnterDoneCB = null)
        {
            base.Enter(onEnterDoneCB);

            lumen_Text.text = LOAGameManager.Instance.playerProfile.lumen.ToString();
            platinum_Text.text = LOAGameManager.Instance.playerProfile.platinum.ToString();
        }

        public void OnBuildClick()
        {
            LOAGameManager.Instance.uIManager.ChangePanel(PanelType.Build);
        }

        public void OnLandClick()
        {
            LOAGameManager.Instance.uIManager.ChangePanel(PanelType.Land);
        }

        public void OnStoreCLick()
        {
            Debug.Log("OnStoreCLick");
        }

        void OnLumenChange(int newValue)
        {
            lumen_Text.text = newValue.ToString();
        }

        void OnPlatinumChange(int newValue)
        {
            platinum_Text.text = newValue.ToString();
        }
    }

}
