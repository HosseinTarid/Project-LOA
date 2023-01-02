using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class BackToRTSCameraPanel : UIBasePanel
    {
        public override void Exit(Action onExitDoneCB = null)
        {
            base.Exit(onExitDoneCB);
            LOAGameManager.Instance.CameraController.SetCurrentState(CameraMovement.CameraState.RTS);
        }
    }
}