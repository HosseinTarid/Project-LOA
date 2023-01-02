using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITester : MonoBehaviour
{
    private void Start()
    {
        UI.MainHUD.MainHUDState.Instance.Show();
    }
}
