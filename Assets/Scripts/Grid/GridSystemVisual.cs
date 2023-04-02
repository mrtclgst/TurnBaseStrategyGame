using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform _gridVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> _gridVisualTypeMaterialList;
    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType m_GridVisualType;
        public Material m_Material;
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft, //for shooting range
        Yellow
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GridSystemVisual!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        int width = LevelGrid.Instance.GetWidth();
        int height = LevelGrid.Instance.GetHeight();
        _gridSystemVisualSingleArray = new GridSystemVisualSingle[width, height];

        GameObject visualContainer = GameObject.Find("Grid Visuals");
        if (!visualContainer)
        {
            visualContainer = new GameObject("Grid Visuals");
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(_gridVisualSinglePrefab,
                    LevelGrid.Instance.GetWorldPosition(gridPosition),
                    Quaternion.identity, visualContainer.transform);
                _gridSystemVisualSingleArray[x, z] =
                    gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        LevelGrid.Instance.OnEventAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UpdateGridVisual();
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                _gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition._x, gridPosition._z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    void UpdateGridVisual()
    {
        HideAllGridPosition();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        GridVisualType gridVisualType;

        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
        }

        ShowGridPositionList(
            selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = 0; x <= range; x++)
        {
            for (int z = 0; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition()
    {
        UpdateGridVisual();
    }

    private Material GetGridVisualTypeMaterial(GridVisualType type)
    {
        for (int index = 0; index < _gridVisualTypeMaterialList.Count; index++)
        {
            if (_gridVisualTypeMaterialList[index].m_GridVisualType == type)
            {
                return _gridVisualTypeMaterialList[index].m_Material;
            }
        }

        return null;
    }
}