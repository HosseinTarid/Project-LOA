using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.BuildingInfoPopup
{

    public class BuildingInfoPopupView : MonoBehaviour
    {

        [field: SerializeField] public GameObject InfoPopupGO { private set; get; }
        [field: SerializeField] public GameObject StatisticPopupGO { private set; get; }
        [field: SerializeField] public TMP_Text[] InfoTexts { private set; get; }
        [field: SerializeField] public Image[] ItemImages { private set; get; }
        [field: SerializeField] public Image FirstStatisticBar { private set; get; }
        [field: SerializeField] public Image SecondStatisticBar { private set; get; }
        [field: SerializeField] public Image ThirdStatisticBar { private set; get; }

    }

}