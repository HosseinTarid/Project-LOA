using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        Pool<GridCell> cellPool = new Pool<GridCell>();
        [SerializeField]
        Vector2Int gridSize;
        [SerializeField]
        float cellSize;
        public float CellSize => cellSize;
        [SerializeField]
        float gridHeight = 3f;


        float cellHalfSize;


        List<GridCell> activeCells = new List<GridCell>();

        public void CreatGrid()
        {
            ReleaseAllActiveCells();
            cellHalfSize = cellSize / 2;

            for (int i = 0; i < gridSize.y; i++)
            {
                for (int j = 0; j < gridSize.x; j++)
                {
                    GridCell cell = cellPool.GetActive;
                    cell.SetCellData(new Vector2Int(j, i), cellSize);
                    cell.transform.position = new Vector3(transform.position.x + cellHalfSize + j * cellSize, gridHeight, transform.position.z + cellHalfSize + i * cellSize);
                    cell.transform.eulerAngles = new Vector3(90f, 0, 0);
                    activeCells.Add(cell);
                }
            }

            ClearGrid();
        }

        public void ClearGrid()
        {
            activeCells.ForEach(x => x.isOccupied = false);
        }

        public void ClearSpace(Vector2Int startIndex, Vector2Int size)
        {
            for (int y = startIndex.y; y < startIndex.y + size.y; y++)
                for (int x = startIndex.x; x < startIndex.x + size.x; x++)
                    activeCells[GetCellIndex(x, y)].isOccupied = false;
        }

        void ReleaseAllActiveCells()
        {
            for (int i = activeCells.Count - 1; i >= 0; i--)
                activeCells[i].gameObject.SetActive(false);

            activeCells.Clear();
        }

        public Vector3 GetCellPosition(Vector2Int cellIndex)
        {
            return transform.position + new Vector3(cellIndex.x * cellSize, 0, cellIndex.y * cellSize); ;
        }

        public bool GetFirstEmptySpace(Vector2Int size, out Vector2Int firstCellIndex, out Vector3 firstCellPosition)
        {
            firstCellPosition = Vector3.zero;
            firstCellIndex = Vector2Int.zero;

            /*From bottom to top, left to right*/
            /* for (int i = 0; i < gridSize.y - (size.y - 1); i++)
            {
                for (int j = 0; j < gridSize.x - (size.x - 1); j++)
                {
                    if (IsAreaEmpty(new Vector2Int(j, i), size))
                    {
                        firstCellPosition = transform.position + new Vector3(j * cellSize, 0, i * cellSize);
                        firstCellIndex = new Vector2Int(j, i);
                        return true;
                    }
                }
            } */

            /*Spiral From middle*/
            // (di, dj) is a vector - direction in which we move right now
            int di = 1;
            int dj = 0;
            // length of current segment
            int segment_length = 1;

            // current position (i, j) and how much of current segment we passed
            int i = gridSize.x / 2 - 1;
            int j = gridSize.y / 2 - 1;
            int segment_passed = 0;
            int cellCount = gridSize.x * gridSize.y;
            for (int k = 0; k < cellCount; ++k)
            {
                if (IsAreaEmpty(new Vector2Int(i, j), size))
                {
                    firstCellPosition = GetCellPosition(new Vector2Int(i, j));
                    firstCellIndex = new Vector2Int(i, j);
                    return true;
                }

                // make a step, add 'direction' vector (di, dj) to current position (i, j)
                i += di;
                j += dj;
                ++segment_passed;

                if (segment_passed == segment_length)
                {
                    // done with current segment
                    segment_passed = 0;

                    // 'rotate' directions
                    int buffer = di;
                    di = -dj;
                    dj = buffer;

                    // increase segment length if necessary
                    if (dj == 0)
                        ++segment_length;
                }
            }

            return false;
        }

        public bool CheckCellsAndSetColor(Vector2Int index, Vector2Int size)
        {
            if (index.x + size.x > gridSize.x)
                index.x = gridSize.x - size.x;

            if (index.y + size.y > gridSize.y)
                index.y = gridSize.y - size.y;

            ResetCellsColor(true);

            bool isAreaEmpty = IsAreaEmpty(index, size);

            for (int y = index.y; y < index.y + size.y; y++)
                for (int x = index.x; x < index.x + size.x; x++)
                    activeCells[GetCellIndex(x, y)].SetColor(isAreaEmpty);

            return isAreaEmpty;
        }

        bool IsAreaEmpty(Vector2Int startIndex, Vector2Int size)
        {
            if (startIndex.x + size.x > gridSize.x || startIndex.y + size.y > gridSize.y)
                return false;

            bool result = true;
            for (int y = startIndex.y; y < startIndex.y + size.y && result; y++)
                for (int x = startIndex.x; x < startIndex.x + size.x && result; x++)
                    result &= !activeCells[GetCellIndex(x, y)].isOccupied;

            return result;
        }

        int GetCellIndex(int x, int y) => y * gridSize.x + x;
        int GetCellIndex(Vector2Int index) => GetCellIndex(index.x, index.y);

        public void ResetCellsColor(bool showOccupied = false)
        {
            foreach (var cell in activeCells)
                cell.ResetColor(showOccupied);
        }

        public Vector2Int GetNearestCellIndex(Vector3 position, Vector2Int buildingSize)
        {
            Vector2Int cellIndex = Vector2Int.zero;
            float shortestDistance = int.MaxValue;
            float temp;
            float currentDistance;

            for (int y = 0; y < gridSize.y; y++)
            {
                temp = transform.position.z + y * cellSize + cellHalfSize;
                currentDistance = Mathf.Abs(position.z - temp);
                if (currentDistance < shortestDistance)
                {
                    cellIndex.y = y;
                    shortestDistance = currentDistance;
                }
            }

            shortestDistance = int.MaxValue;
            for (int x = 0; x < gridSize.x; x++)
            {
                temp = transform.position.x + x * cellSize + cellHalfSize;
                currentDistance = Mathf.Abs(position.x - temp);
                if (currentDistance < shortestDistance)
                {
                    cellIndex.x = x;
                    shortestDistance = currentDistance;
                }
            }

            cellIndex.x = Mathf.Clamp(cellIndex.x - buildingSize.x / 2, 0, gridSize.x - buildingSize.x);
            cellIndex.y = Mathf.Clamp(cellIndex.y - buildingSize.y / 2, 0, gridSize.y - buildingSize.y);
            return cellIndex;
        }

        public bool OccupySpace(Vector2Int startIndex, Vector2Int size)
        {
            if (IsAreaEmpty(startIndex, size))
            {
                for (int y = startIndex.y; y < startIndex.y + size.y; y++)
                    for (int x = startIndex.x; x < startIndex.x + size.x; x++)
                        activeCells[GetCellIndex(x, y)].isOccupied = true;
                return true;
            }

            return false;
        }
    }
}
