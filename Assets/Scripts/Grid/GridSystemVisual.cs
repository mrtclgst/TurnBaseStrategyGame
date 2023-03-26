using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform _gridVisualSinglePrefab;
    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

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
    }

    private void Update()
    {
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

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition._x, gridPosition._z].Show();
        }
    }

    void UpdateGridVisual()
    {
        HideAllGridPosition();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        ShowGridPositionList(
            selectedAction.GetValidActionGridPositionList());
    }
}