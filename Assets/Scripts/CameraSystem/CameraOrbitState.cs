using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Patterns;
using System;

namespace CameraMovement
{
    public class CameraOrbitState : CameraStateBase
    {
        public CameraOrbitState(FiniteStateMachine<CameraState> StateMachine, CameraController controller) : base(CameraState.Orbit, StateMachine, controller) { }

        float cameraSpeed = 0.5f;
        float cameraScrollMultiplier = 1f;

        private float stepSize = 2f;
        private float zoomDampening = 7.5f;
        private float minHeight = 18f;
        private float maxHeight = 25f;
        private float zoomSpeed = 5f;
        private float zoomHeight;

        float mouseStartPosition;
        float baseStartRotation;
        private CameraControls cameraActions;

        float minDistance;
        float maxDistance;
        bool isInited = false;
        Vector3 cameraTargetPosition;

        public override void Enter()
        {
            base.Enter();

            if (!isInited)
            {
                cameraActions = new CameraControls();
                isInited = true;
            }

            zoomHeight = 20;
            minDistance = controller.cameraTransform.localPosition.magnitude - 5;
            maxDistance = minDistance + 10;
            //LookAtObject();

            cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
            cameraActions.Camera.Enable();
        }

        public override void Exit()
        {
            base.Exit();

            cameraActions.Camera.ZoomCamera.performed -= ZoomCamera;
            cameraActions.Camera.Disable();
        }

        public override void Update()
        {
            base.Update();

            UpdateRotation();
            UpdateCameraPosition();
        }

        public override CameraPosition GetCameraPosition()
        {
            if (hasEntered)
                return new CameraPosition(controller.cameraTransform.localPosition, controller.baseTransform.position, controller.cameraTransform.rotation, controller.baseTransform.rotation);
            else
                return new CameraPosition(new Vector3(0, 20, -20), controller.orbitObject.position + new Vector3(0, 5, 0), Quaternion.LookRotation(new Vector3(0, -20, 20)), new Quaternion().normalized);
        }

        /* private void ZoomCamera(InputAction.CallbackContext input)
         {
             float inputValue = input.ReadValue<Vector2>().y / 100;

             if (Mathf.Abs(inputValue) > 0.1f)
                 cameraTargetPosition += controller.cameraTransform.forward * inputValue * cameraScrollMultiplier;
         }*/

        void LookAtObject()
        {
            controller.cameraTransform.LookAt(controller.baseTransform);
        }

        bool touchFirstFrame;
        void UpdateRotation()
        {
#if UNITY_EDITOR
            if (Mouse.current.leftButton.isPressed)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    baseStartRotation = controller.baseTransform.eulerAngles.y;
                    mouseStartPosition = Mouse.current.position.ReadValue().x;
                }
                else
                {
                    float diff = mouseStartPosition - Mouse.current.position.ReadValue().x;
                    controller.baseTransform.rotation = Quaternion.Euler(0f, diff * cameraSpeed + baseStartRotation, 0f);
                }
            }
#endif


#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.touchCount == 1)
            {
                if (!touchFirstFrame)
                {
                    touchFirstFrame = true;
                    baseStartRotation = controller.baseTransform.eulerAngles.y;
                    mouseStartPosition = Input.GetTouch(0).position.x;
                }
                else
                {
                    float diff = mouseStartPosition - Input.GetTouch(0).position.x;
                    controller.baseTransform.rotation = Quaternion.Euler(0f, diff * cameraSpeed + baseStartRotation, 0f);
                }
            }
            else
                touchFirstFrame = false;
#endif
        }

        private void ZoomCamera(InputAction.CallbackContext obj)
        {
            float inputValue = -obj.ReadValue<Vector2>().y / 200f;

            if (Mathf.Abs(inputValue) > 0.1f)
            {
                zoomHeight = controller.cameraTransform.localPosition.y + inputValue * stepSize;

                if (zoomHeight < minHeight)
                    zoomHeight = minHeight;
                else if (zoomHeight > maxHeight)
                    zoomHeight = maxHeight;
            }
        }

        private void UpdateCameraPosition()
        {
            //set zoom target
            Vector3 zoomTarget = new Vector3(controller.cameraTransform.localPosition.x, zoomHeight, controller.cameraTransform.localPosition.z);
            //add vector for forward/backward zoom
            zoomTarget -= zoomSpeed * (zoomHeight - controller.cameraTransform.localPosition.y) * Vector3.forward;

            controller.cameraTransform.localPosition = Vector3.Lerp(controller.cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
            LookAtObject();
        }
    }
}
