using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{
    public class GridCell : MonoBehaviour
    {
        [HideInInspector]
        public Vector2Int positionInGrid { private set; get; }

        [HideInInspector]
        public bool isOccupied;

        [SerializeField]
        SpriteRenderer _sr;
        [SerializeField]
        Color darkColor;
        [SerializeField]
        Color lightColor;
        [SerializeField]
        Color availableColor;
        [SerializeField]
        Color unavailableColor;
        [SerializeField]
        Color occupiedColor;

        public void SetCellData(Vector2Int pos, float cellSize)
        {
            positionInGrid = pos;
            isOccupied = false;
            //setSize
            Vector3 scale = _sr.transform.localScale;
            scale.x /= _sr.bounds.size.x;
            scale.y /= _sr.bounds.size.y;
            scale *= cellSize;
            _sr.transform.localScale = scale;
            //setColor
            ResetColor();
        }

        public void SetColor(bool isAvalilable) => _sr.color = isAvalilable ? availableColor : unavailableColor;
        public void ResetColor(bool showOccupied = false) => _sr.color = (showOccupied && isOccupied) ? occupiedColor : ((Mathf.Abs(positionInGrid.x - positionInGrid.y) % 2 == 0) ? darkColor : lightColor);
    }
}
