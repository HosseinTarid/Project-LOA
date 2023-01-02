using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.MainHUD
{

    public class MainHUDView : MonoBehaviour
    {

        [field: SerializeField] public Button InventoryBtn { private set; get; }
        [field: SerializeField] public Button OnlineMarketBtn { private set; get; }
        [field: SerializeField] public TMP_Text LumenTxt { private set; get; }
        [field: SerializeField] public TMP_Text PlatiniumTxt { private set; get; }

    }

}