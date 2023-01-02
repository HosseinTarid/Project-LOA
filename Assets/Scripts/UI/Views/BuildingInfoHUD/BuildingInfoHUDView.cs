using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.BuildingInfoHUD
{

    public class BuildingInfoHUDView : MonoBehaviour
    {

        [field: SerializeField] public TMP_Text NameText { private set; get; }
        [field: SerializeField] public Button InfoButton { private set; get; }
        [field: SerializeField] public Button FeatureButton { private set; get; }
        [field: SerializeField] public Button DestroyButton { private set; get; }
        [field: SerializeField] public Button RepairButton { private set; get; }

    }

}