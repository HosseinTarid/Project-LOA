using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Android;
using Patterns;
using System;

namespace CameraMovement
{
    public class CameraRTSState : CameraStateBase
    {
        public CameraRTSState(FiniteStateMachine<CameraState> StateMachine, CameraController controller) : base(CameraState.RTS, StateMachine, controller) { }

        const float CameraMoveDuration = 0.5f;

        private CameraControls cameraActions;
        private InputAction movement;
        private Transform cameraTransform;
        private Transform baseTransform;

        private float edgeTolerance = 0.1f;

        private float maxSpeed = 5f;
        private float speed;
        private float acceleration = 10f;
        private float damping = 15f;

        private float stepSize = 2f;
        private float zoomDampening = 7.5f;
        private float minHeight = 50f;
        private float maxHeight = 130f;
        private float zoomSpeed = 17f;

        //value set in various functions 
        //used to update the position of the camera base object.
        private Vector3 targetPosition;

        private float zoomHeight;

        //used to track and maintain velocity w/o a rigidbody
        private Vector3 horizontalVelocity;
        private Vector3 lastPosition;

        //tracks where the dragging action started
        Vector3 startDrag;
        Vector3 cameraExitPosition;
        Vector3 baseExitPosition;
        Quaternion cameraRotation;

        bool isInited = false;
        float lastDiableTime;

        public override void Enter()
        {
            base.Enter();

            if (!isInited)
                Initialize();

            zoomHeight = cameraTransform.localPosition.y;
            cameraTransform.LookAt(baseTransform);

            lastPosition = baseTransform.position;

            movement = cameraActions.Camera.MoveCamera;
            cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
            cameraActions.Camera.Enable();

            LOAGameManager.Instance.buildingsManager.OnBuildingSelectedEvent += MoveCameraTo;
        }

        public override void Exit()
        {
            base.Exit();

            cameraActions.Camera.ZoomCamera.performed -= ZoomCamera;
            cameraActions.Camera.Disable();

            cameraExitPosition = cameraTransform.localPosition;
            baseExitPosition = baseTransform.position;
            cameraRotation = cameraTransform.rotation;

            LOAGameManager.Instance.buildingsManager.OnBuildingSelectedEvent -= MoveCameraTo;
        }

        public override void Update()
        {
            base.Update();

            if (LOAGameManager.Instance.uIManager.CurrentPanel.Value != UI.PanelType.Main &&
                LOAGameManager.Instance.uIManager.CurrentPanel.Value != UI.PanelType.BuildingPlacement)
            {
                lastDiableTime = Time.time;
                return;
            }

            //inputs
            if (LOAGameManager.Instance.buildingsManager.IsDraggingBuilding)
            {
                lastDiableTime = Time.time;
                CheckMouseAtScreenEdge();
            }
            else
            {
                if (Time.time - lastDiableTime < 0.5f)
                    return;

                DragCamera();

#if UNITY_ANDROID && !UNITY_EDITOR
                TouchZoomCamera();
#endif
            }

            //move base and camera objects
            UpdateVelocity();
            UpdateBasePosition();
            UpdateCameraPosition();
        }

        public override CameraPosition GetCameraPosition()
        {
            if (isInited)
                if (hasEntered)
                    return new CameraPosition(cameraTransform.localPosition, baseTransform.position, cameraTransform.rotation, new Quaternion().normalized);
                else
                    return new CameraPosition(cameraExitPosition, baseExitPosition, cameraRotation, new Quaternion().normalized);
            else
                return new CameraPosition(controller.cameraTransform.localPosition, controller.baseTransform.position, controller.cameraTransform.rotation, new Quaternion().normalized);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        private void Initialize()
        {
            cameraActions = new CameraControls();
            cameraTransform = controller.cameraTransform;
            baseTransform = controller.baseTransform;
            isInited = true;
        }

        private void UpdateVelocity()
        {
            horizontalVelocity = (baseTransform.position - lastPosition) / Time.deltaTime;
            horizontalVelocity.y = 0f;
            lastPosition = baseTransform.position;
        }

        bool touchFirstFrame;
        private void DragCamera()
        {
#if UNITY_EDITOR
            if (!Mouse.current.leftButton.isPressed)
                return;

            //create plane to raycast to
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (plane.Raycast(ray, out float distance))
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                    startDrag = ray.GetPoint(distance);
                else
                    targetPosition += startDrag - ray.GetPoint(distance);
            }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.touchCount != 1)
            {
                touchFirstFrame = false;
                return;
            }

            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (plane.Raycast(ray, out float distance))
            {
                if (!touchFirstFrame)
                {
                    touchFirstFrame = true;
                    startDrag = ray.GetPoint(distance);
                }
                else
                    targetPosition += startDrag - ray.GetPoint(distance);
            }
#endif
        }

        private void CheckMouseAtScreenEdge()
        {
            //mouse position is in pixels
            Vector2 mousePosition = Vector2.zero;
#if UNITY_EDITOR
            mousePosition = Mouse.current.position.ReadValue();
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.touchCount != 1)
                return;
            mousePosition = Input.GetTouch(0).position;
#endif

            Vector3 moveDirection = Vector3.zero;

            //horizontal scrolling
            if (mousePosition.x < edgeTolerance * Screen.width)
                moveDirection += -GetCameraRight();
            else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
                moveDirection += GetCameraRight();

            //vertical scrolling
            if (mousePosition.y < edgeTolerance * Screen.height)
                moveDirection += -GetCameraForward();
            else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
                moveDirection += GetCameraForward();

            targetPosition += moveDirection;
        }

        private void UpdateBasePosition()
        {
            Vector3 finalPosition;
            if (targetPosition.sqrMagnitude > 0.1f)
            {
                //create a ramp up or acceleration
                speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
                finalPosition = baseTransform.position + targetPosition * speed * Time.deltaTime;
            }
            else
            {
                //create smooth slow down
                horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
                finalPosition = baseTransform.position + horizontalVelocity * Time.deltaTime;
            }

            finalPosition.x = Mathf.Clamp(finalPosition.x, -200, 150);
            finalPosition.z = Mathf.Clamp(finalPosition.z, -200, 150);
            baseTransform.position = finalPosition;

            //reset for next frame
            targetPosition = Vector3.zero;
        }

        private void ZoomCamera(InputAction.CallbackContext obj)
        {
            float inputValue = -obj.ReadValue<Vector2>().y / 100f;

            if (Mathf.Abs(inputValue) > 0.1f)
            {
                zoomHeight = cameraTransform.localPosition.y + inputValue * stepSize;

                if (zoomHeight < minHeight)
                    zoomHeight = minHeight;
                else if (zoomHeight > maxHeight)
                    zoomHeight = maxHeight;
            }
        }

        float touchDistance;
        bool isTouchZoomming;

        void TouchZoomCamera()
        {
            if (Input.touchCount != 2)
            {
                isTouchZoomming = false;
                zoomHeight = cameraTransform.localPosition.y;
                return;
            }

            if (!isTouchZoomming)
            {
                touchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                isTouchZoomming = true;
            }
            else
            {
                float currentTouchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

                zoomHeight = cameraTransform.localPosition.y + (-(currentTouchDistance - touchDistance) / 1000f) * stepSize;

                if (zoomHeight < minHeight)
                    zoomHeight = minHeight;
                else if (zoomHeight > maxHeight)
                    zoomHeight = maxHeight;
            }
        }

        private void UpdateCameraPosition()
        {
            //set zoom target
            Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
            //add vector for forward/backward zoom
            zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * cameraTransform.forward;

            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
            cameraTransform.LookAt(baseTransform);
        }

        //gets the horizontal forward vector of the camera
        private Vector3 GetCameraForward()
        {
            Vector3 forward = cameraTransform.forward * maxSpeed;
            forward.y = 0f;
            return forward;
        }

        //gets the horizontal right vector of the camera
        private Vector3 GetCameraRight()
        {
            Vector3 right = cameraTransform.right * maxSpeed;
            right.y = 0f;
            return right;
        }

        public void MoveCameraTo(Vector2 destination)
        {
            destination.x = Mathf.Clamp(destination.x, -200, 150);
            destination.y = Mathf.Clamp(destination.y, -200, 150);
            LOAGameManager.Instance.StartCoroutine(MovingCamera(destination));
        }

        IEnumerator MovingCamera(Vector2 destination)
        {
            Vector3 startPos = baseTransform.position;
            Vector3 destinationPos = new Vector3(destination.x, startPos.y, destination.y);

            float startTime = Time.time;
            float progress;
            while (Time.time - startTime < CameraMoveDuration)
            {
                progress = (Time.time - startTime) / CameraMoveDuration;
                baseTransform.position = Vector3.Lerp(startPos, destinationPos, progress);
                yield return new WaitForEndOfFrame();
            }

            baseTransform.position = destinationPos;
            lastPosition = baseTransform.position;
        }
    }
}
