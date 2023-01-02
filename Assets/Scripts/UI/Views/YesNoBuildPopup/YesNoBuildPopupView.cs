using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.YesNoBuildPopup
{

    public class YesNoBuildPopupView : MonoBehaviour
    {

        [field: SerializeField] public RectTransform CanvasRoot { private set; get; }
        [field: SerializeField] public RectTransform PopupRoot { private set; get; }
        [field: SerializeField] public Button AcceptButton { private set; get; }
        [field: SerializeField] public Button CancelButton { private set; get; }
        [field: SerializeField] public Button ArrowRightButton { private set; get; }
        [field: SerializeField] public Button ArrowLeftButton { private set; get; }
        [field: SerializeField] public Button ArrowUpButton { private set; get; }
        [field: SerializeField] public Button ArrowBottomButton { private set; get; }

    }

}