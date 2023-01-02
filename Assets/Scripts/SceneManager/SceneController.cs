using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController 
{
    public void Init()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene prev, Scene current)
    {
        if (current.name == "Grid")
            LOAGameManager.Instance.uIManager.ChangePanel(UI.PanelType.Main);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Grid", LoadSceneMode.Single);
    }
}

