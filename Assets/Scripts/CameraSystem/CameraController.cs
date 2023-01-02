using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using UI;


namespace CameraMovement
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        public Transform baseTransform;
        [SerializeField]
        public Transform cameraTransform;
        [SerializeField]
        [Range(0.1f, 5f)]
        float changeStateDuration = 1;
        [SerializeField]
        AnimationCurve transitionCurve;

        FiniteStateMachine<CameraState> cameraFSM;

        [HideInInspector]
        public Transform orbitObject;

        bool isChangingState;
        CameraState nextState;
        CameraPosition previousStateCameraPosition;

        public event Action<bool> OnOrbitCameraActiveEvent;

        protected void Awake()
        {
            cameraFSM = new FiniteStateMachine<CameraState>();
            cameraFSM.Add(new CameraRTSState(cameraFSM, this));
            cameraFSM.Add(new CameraOrbitState(cameraFSM, this));

            SetCurrentState(CameraState.RTS);
        }

        public void SetCurrentState(CameraState state)
        {
            if (isChangingState)
                return;

            if (cameraFSM.CurrentStateID == state)
                return;

            if (!cameraFSM.HasAState)
            {
                SetCameraPosition(((CameraStateBase)cameraFSM.GetState(state)).GetCameraPosition());
                cameraFSM.EnterState(state);
            }
            else
            {
                previousStateCameraPosition = ((CameraStateBase)cameraFSM.CurrentState).GetCameraPosition();
                cameraFSM.ExitCurrentState();
                nextState = state;

                if (nextState == CameraState.Orbit)
                    LOAGameManager.Instance.uIManager.ChangePanel(PanelType.BackToRTSCamera);
                if (nextState == CameraState.RTS)
                    LOAGameManager.Instance.uIManager.ChangePanel(PanelType.Main);

                OnOrbitCameraActiveEvent?.Invoke(nextState == CameraState.Orbit);

                ChangeCameraPosition();
            }
        }

        void Update()
        {
            if (cameraFSM != null)
                cameraFSM.Update();
        }

        void FixedUpdate()
        {
            if (cameraFSM != null)
                cameraFSM.FixedUpdate();
        }

        public void SetCameraPosition(CameraPosition cameraPosition)
        {
            cameraTransform.localPosition = cameraPosition.cameraPostion;
            cameraTransform.rotation = cameraPosition.cameraRotation;
            baseTransform.localPosition = cameraPosition.basePosition;
            baseTransform.rotation = cameraPosition.baseRotation;
        }

        void ChangeCameraPosition()
        {
            StartCoroutine(ChangingCameraPosition());
        }

        IEnumerator ChangingCameraPosition()
        {
            isChangingState = true;
            float startTime = Time.time;
            float endTime = startTime + changeStateDuration;
            CameraPosition nextStateCameraPosition = ((CameraStateBase)cameraFSM.GetState(nextState)).GetCameraPosition();
            float relativeValue = 0;

            while (Time.time < endTime)
            {
                relativeValue = transitionCurve.Evaluate((Time.time - startTime) / changeStateDuration);
                SetCameraPosition(new CameraPosition(Vector3.Lerp(previousStateCameraPosition.cameraPostion, nextStateCameraPosition.cameraPostion, relativeValue),
                                  Vector3.Lerp(previousStateCameraPosition.basePosition, nextStateCameraPosition.basePosition, relativeValue),
                                  Quaternion.Lerp(previousStateCameraPosition.cameraRotation, nextStateCameraPosition.cameraRotation, relativeValue),
                                  Quaternion.Lerp(previousStateCameraPosition.baseRotation, nextStateCameraPosition.baseRotation, relativeValue)));

                yield return new WaitForEndOfFrame();
            }

            SetCameraPosition(nextStateCameraPosition);
            //EnterNextState
            cameraFSM.EnterState(nextState);
            isChangingState = false;
        }
    }
}
