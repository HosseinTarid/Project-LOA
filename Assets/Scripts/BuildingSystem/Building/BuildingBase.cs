using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Script.Services.API_Service.Game.User;

namespace BuildingSystem
{
    public abstract class BuildingBase : MonoBehaviour
    {
        [SerializeField]
        protected BuildingType type;
        public BuildingType Type => type;
        [SerializeField]
        protected int level;
        public int Level => level;
        [SerializeField]
        protected Vector2Int size;
        public Vector2Int Size => size;
        [SerializeField]
        Vector3 buildingUIPosition;

        [HideInInspector]
        public bool isPlacing;
        Vector3 offSet;
        bool isDragging;
        public bool IsDragging => isDragging;
        Vector3 placingPosition;
        protected BuildingUI buildingUI;

        int buildingId;
        public int BuildingID => buildingId;

        Coroutine coroutine;
        public void Init(int buildingId)
        {
            this.buildingId = buildingId;
            var building = LOAGameManager.Instance.playerProfile.activeLand.GetBuilding(buildingId);

            if (buildingUI == null)
            {
                buildingUI = Instantiate(BuildingsManager.BuildingUIPrefab, transform);
                buildingUI.transform.localPosition = buildingUIPosition;
            }

            if (coroutine != null)
                StopCoroutine(coroutine);

            if (building == null || building.Status == BuildingStatus.ready)
            {
                if (type == BuildingType.apartment)
                    buildingUI.ShowResourceSign();
                else
                    buildingUI.DisableAll();
            }
            else if (building.Status == BuildingStatus.constructing)
            {
                buildingUI.ShowConstructSign();
                coroutine = StartCoroutine(Constructing(building));
            }
            else
            {
                buildingUI.ShowDeconstructSign();
                coroutine = StartCoroutine(Deconstructing(building));
            }

            LOAGameManager.Instance.CameraController.OnOrbitCameraActiveEvent += OnOrbitCameraActive;
        }

        void OnDisable()
        {
            if (LOAGameManager.Instance != null)
                LOAGameManager.Instance.CameraController.OnOrbitCameraActiveEvent -= OnOrbitCameraActive;
        }

        void OnOrbitCameraActive(bool value) => buildingUI.SetUIActive(!value);

        IEnumerator Constructing(GameUserProfile.Building building)
        {
            while (building.Construct_remaining_time > 0)
                yield return new WaitForSeconds(0.5f);

            if (LOAGameManager.Instance.buildingsManager.selectedBuildingId == buildingId && LOAGameManager.Instance.uIManager.CurrentPanel.Value == UI.PanelType.BuildingConstruction)
                LOAGameManager.Instance.uIManager.OnBackClick();
            Init(buildingId);
        }

        IEnumerator Deconstructing(GameUserProfile.Building building)
        {
            while (building.Destruct_remaining_time > 0)
                yield return new WaitForSeconds(1);

            if (LOAGameManager.Instance.buildingsManager.selectedBuildingId == buildingId && LOAGameManager.Instance.uIManager.CurrentPanel.Value == UI.PanelType.BuildingConstruction)
                LOAGameManager.Instance.uIManager.OnBackClick();
            LOAGameManager.Instance.playerProfile.OnBuildingDeconstructed(buildingId);
        }

        void OnMouseUpAsButton()
        {
            if (isPlacing || LOAGameManager.Instance.buildingsManager.IsInDraggingMode || LOAGameManager.Instance.uIManager.CurrentPanel != UI.PanelType.Main)
                return;

            LOAGameManager.Instance.CameraController.orbitObject = transform;

            if (LOAGameManager.Instance.uIManager.CurrentPanel.HasValue && LOAGameManager.Instance.uIManager.CurrentPanel.Value != UI.PanelType.BackToRTSCamera)
            {
                LOAGameManager.Instance.buildingsManager.OnSelectBuilding(buildingId);
                var building = LOAGameManager.Instance.playerProfile.activeLand.GetBuilding(buildingId);

                if (building.Status == BuildingStatus.constructing)
                    LOAGameManager.Instance.uIManager.ChangePanel(UI.PanelType.BuildingConstruction);
                else if (building.Status == BuildingStatus.ready)
                    LOAGameManager.Instance.uIManager.ChangePanel(UI.PanelType.BuildingAction);
            }
        }

        void OnMouseUp()
        {
            isDragging = false;
        }

        void OnMouseDrag()
        {

#if UNITY_EDITOR
            if (!isPlacing)
                return;

            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (plane.Raycast(ray, out float distance))
            {
                if (!isDragging)
                {
                    isDragging = true;
                    offSet = transform.position - ray.GetPoint(distance);
                }

                LOAGameManager.Instance.buildingsManager.OnBuildingPlacing(/*offSet +*/ ray.GetPoint(distance), size);
            }
#endif
        }

        void Update()
        {
            if (isPlacing)
                OnTouchDrag();

        }

        bool isTouchDragging;
        bool touchFirstFrame;
        void OnTouchDrag()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.touchCount != 1)
            {
                touchFirstFrame = false;
                isTouchDragging = false;
                isDragging = false;
                return;
            }

            if (!isTouchDragging)
            {
                Touch touch = Input.touches[0];
                Vector3 pos = touch.position;
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(pos);
                if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                    isTouchDragging = true;
                
            }

            if (isTouchDragging)
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                if (plane.Raycast(ray, out float distance))
                {
                    if (!isDragging)
                    {
                        isDragging = true;
                        offSet = transform.position - ray.GetPoint(distance);
                    }

                    LOAGameManager.Instance.buildingsManager.OnBuildingPlacing(/*offSet +*/ ray.GetPoint(distance), size);
                }
            }
#endif
        }
    }
}
