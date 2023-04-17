using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemHex<TGridObject>
{
    private const float HEX_VERTICAL_OFFSET_MULTIPLIER = 0.75f;
    private int _width;
    private int _height;
    private float _cellSize;
    private TGridObject[,] _gridObjectArray;

    public GridSystemHex(int width, int height, float cellSize,
        Func<GridSystemHex<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _gridObjectArray = new TGridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.white, 1000);

                GridPosition gridPosition = new GridPosition(x, z);
                _gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition._x, 0, 0) * _cellSize
               + new Vector3(0, 0, gridPosition._z) * _cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER
               + (((gridPosition._z % 2) == 1)
                   ? new Vector3(1, 0, 0) * _cellSize * 0.5f
                   : Vector3.zero);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        GridPosition roughXZ = new GridPosition
        (
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z / _cellSize / HEX_VERTICAL_OFFSET_MULTIPLIER)
        );

        bool oddRow = roughXZ._z % 2 == 1;
        List<GridPosition> neighbourGridPositionList = new List<GridPosition>()
        {
            roughXZ.East,
            roughXZ.West,
            roughXZ.North,
            roughXZ.South,
            roughXZ + new GridPosition(oddRow ? +1 : -1, 1),
            roughXZ + new GridPosition(oddRow ? +1 : -1, -1),
        };

        GridPosition closestGridPosition = roughXZ;
        foreach (GridPosition neighbourGridPosition in neighbourGridPositionList)
        {
            if (Vector3.Distance(worldPosition, GetWorldPosition(neighbourGridPosition)) <
                Vector3.Distance(worldPosition, GetWorldPosition(closestGridPosition)))
            {
                closestGridPosition = neighbourGridPosition;
            }
        }

        return closestGridPosition;
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        GameObject debugObjectsParent = GameObject.Find("DebugObjectsParent");
        if (!debugObjectsParent)
        {
            debugObjectsParent = new GameObject("DebugObjectsParent");
        }

        for (int x = 0; x < _height; x++)
        {
            for (int z = 0; z < _width; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugTransform =
                    GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                debugTransform.parent = debugObjectsParent.transform;
                debugTransform.name = $"DebugObject_{x}_{z}";
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition._x, gridPosition._z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition._x >= 0 &&
               gridPosition._z >= 0 &&
               gridPosition._x < _width &&
               gridPosition._z < _height;
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }
}