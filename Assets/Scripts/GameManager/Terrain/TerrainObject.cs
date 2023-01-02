using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LOATerrain
{
    public class TerrainObject : MonoBehaviour
    {
        [SerializeField]
        TerrainType type;
        public TerrainType Type => type;
        [SerializeField]
        Transform cameraBasePoint;
        public Transform CameraBasePoint => cameraBasePoint;
        [SerializeField]
        Transform cameraPoint;
        public Transform CameraPoint => cameraPoint;
        [SerializeField]
        Transform cameraInPoint;
        public Transform CameraInPoint => cameraInPoint;
    }
}