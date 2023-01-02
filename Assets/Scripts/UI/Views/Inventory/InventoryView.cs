using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.Inventory
{

    public class InventoryView : MonoBehaviour
    {

        [field: SerializeField] public Button CloseBtn { private set; get; }
        [field: SerializeField] public Button BuildingTabBtn { private set; get; }
        [field: SerializeField] public Button DecorationTabBtn { private set; get; }
        [field: SerializeField] public GameObject BuildingListGO { private set; get; }
        [field: SerializeField] public GameObject DecorationListGO { private set; get; }

    }

}