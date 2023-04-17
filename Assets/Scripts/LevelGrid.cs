using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public event Action OnEventAnyUnitMovedGridPosition;
    [SerializeField] private Transform _gridDebugObject;
    private GridSystemHex<GridObject> _gridSystemHex;
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _cellSize = 2f;
    public static LevelGrid Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one LevelGrid!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _gridSystemHex = new GridSystemHex<GridObject>(_width, _height, _cellSize,
            (GridSystemHex<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        //_gridSystem.CreateDebugObjects(_gridDebugObject);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(_width, _height, _cellSize);
    }

    public void AddUnitAtPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystemHex.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystemHex.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = _gridSystemHex.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtPosition(fromGridPosition, unit);
        AddUnitAtPosition(toGridPosition, unit);
        OnEventAnyUnitMovedGridPosition?.Invoke();
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return _gridSystemHex.GetGridPosition(worldPosition);
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return _gridSystemHex.GetWorldPosition(gridPosition);
    }

    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystemHex.IsValidGridPosition(gridPosition);
    public int GetWidth() => _gridSystemHex.GetWidth();
    public int GetHeight() => _gridSystemHex.GetHeight();

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystemHex.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystemHex.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = _gridSystemHex.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }

    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = _gridSystemHex.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }
}