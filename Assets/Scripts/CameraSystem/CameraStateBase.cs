using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;

namespace CameraMovement
{
    public enum CameraState { None, RTS, Orbit }
    public abstract class CameraStateBase : State<CameraState>
    {
        protected CameraController controller;
        protected bool hasEntered;

        public CameraStateBase(CameraState id, FiniteStateMachine<CameraState> StateMachine, CameraController controller) : base(id)
        {
            this.StateMachine = StateMachine;
            this.controller = controller;
        }

        public override void Enter()
        {
            hasEntered = true;
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
            hasEntered = false;
        }

        public abstract CameraPosition GetCameraPosition();
    }

    public class CameraPosition
    {
        public Vector3 cameraPostion;
        public Vector3 basePosition;
        public Quaternion cameraRotation;
        public Quaternion baseRotation;

        public CameraPosition(Vector3 cameraPostion, Vector3 basePosition, Quaternion cameraRotation, Quaternion baseRotation)
        {
            this.cameraPostion = cameraPostion;
            this.basePosition = basePosition;
            this.cameraRotation = cameraRotation;
            this.baseRotation = baseRotation;
        }
    }
}
