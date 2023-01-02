using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIManager 
    {
        const string PrefabsAddressPrefix = "UI/";
        const string MainCanvasFileName = "Canvas";
        const string MainEventSystemFileName = "EventSystem";

        Dictionary<PanelType, UIBasePanel> panels = new Dictionary<PanelType, UIBasePanel>();
        List<PanelType> panelsHistory = new List<PanelType>();
        PanelType? currentPanel;
        public PanelType? CurrentPanel => currentPanel;
        Canvas mainCanvas;
        bool isChangingPanel;

        public void Init()
        {
            InstantiateMainCanvas();
            ChangePanel(PanelType.LogIn);
        }

        void InstantiateMainCanvas()
        {
            var canvasPrefab = Resources.Load<Canvas>(PrefabsAddressPrefix + MainCanvasFileName);
            mainCanvas = UnityEngine.Object.Instantiate<Canvas>(canvasPrefab);
            var eventSystemPrefab = Resources.Load(PrefabsAddressPrefix + MainEventSystemFileName);
            UnityEngine.Object.Instantiate(eventSystemPrefab);
        }

        public void ChangePanel(PanelType type, bool WaitForCurrentPanelToExit = false)
        {
            if (currentPanel.Equals(type))
                return;

            if (isChangingPanel)
                return;

            Action OnEnterDone = () => { isChangingPanel = false; };
            Action EnterNextPanel = () =>
            {
                if (!panels.ContainsKey(type))
                    LoadAndInstantiatePanel(type);
                panels[type].Enter(OnEnterDone);
                currentPanel = type;

                //Manage Histroy
                if (type == PanelType.Main)
                    panelsHistory.Clear();
                if (panelsHistory.Count == 0 || (panelsHistory.Count > 0 && panelsHistory[panelsHistory.Count - 1] != type))
                    panelsHistory.Add(type);
            };

            isChangingPanel = true;

            ExitCurrentPanel(WaitForCurrentPanelToExit ? EnterNextPanel : null);

            if (!WaitForCurrentPanelToExit)
            {
                EnterNextPanel();
            }
        }

        void ExitCurrentPanel(Action OnDone)
        {
            if (currentPanel.HasValue)
            {
                panels[currentPanel.Value].Exit(OnDone);
                //panelsHistory.RemoveAt(panelsHistory.Count - 1);
                currentPanel = null;
            }

            OnDone?.Invoke();
        }

        void LoadAndInstantiatePanel(PanelType type)
        {
            string panelPrefabAddress = PrefabsAddressPrefix + type.ToString() + "Panel";
            var prefab = Resources.Load<UIBasePanel>(panelPrefabAddress);
            UIBasePanel newPanel = UnityEngine.Object.Instantiate<UIBasePanel>(prefab, mainCanvas.transform);
            newPanel.enabled = false;
            panels.Add(type, newPanel);
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape) && panels[currentPanel.Value].BackButtonWork)
                OnBackClick();
        }

        public void OnBackClick()
        {
            if (panelsHistory.Count > 1)
            {
                var prevPanel = panelsHistory[panelsHistory.Count - 2];
                ChangePanel(prevPanel);
                if (prevPanel != PanelType.Main)
                {
                    panelsHistory.RemoveAt(panelsHistory.Count - 1);
                    panelsHistory.RemoveAt(panelsHistory.Count - 1);
                }
            }
        }
    }
}