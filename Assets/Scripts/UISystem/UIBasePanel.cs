using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public enum PanelType { LogIn, Main, Build, BuildingInfo, BuildingPlacement, PopUp,
                             BuildingConstruction, BuildingAction, BackToRTSCamera, Land }

    public abstract class UIBasePanel : MonoBehaviour
    {
        protected string showKey = "Show";
        protected string hideKey = "Hide";

        [SerializeField]
        protected PanelType type;
        public PanelType Type => type;
        [SerializeField]
        protected Canvas canvas;
        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected bool backButtonWork = true;
        public bool BackButtonWork => backButtonWork;

        Action onEnterDoneCB;
        Action onExitDoneCB;

        bool entering;
        bool exiting;

        public virtual void Enter(Action onEnterDoneCB = null)
        {
            if(!this.enabled)
                this.enabled = true;
                
            entering = true;
            this.onEnterDoneCB = onEnterDoneCB;
            canvas.enabled = true;
            if (animator != null)
                animator.SetTrigger(showKey);
            else
                OnEnterDone();
        }

        public void OnEnterDone()
        {
            if (entering)
            {
                onEnterDoneCB?.Invoke();
                entering = false;
            }
        }

        public virtual void Exit(Action onExitDoneCB = null)
        {
            exiting = true;
            this.onExitDoneCB = onExitDoneCB;
            if (animator != null)
                animator.SetTrigger(hideKey);
            else
                OnExitDone();
        }

        public void OnExitDone()
        {
            if (exiting)
            {
                canvas.enabled = false;
                onExitDoneCB?.Invoke();
                exiting = false;
            }
        }

        public void OnBackClick()
        {
            LOAGameManager.Instance.uIManager.OnBackClick();
        }
    }
}
