using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI;

public class BuildsPanel : UIBasePanel
{
    public void OnBuildClick()
    {
        LOAGameManager.Instance.uIManager.ChangePanel(PanelType.BuildingInfo);
    }
}
