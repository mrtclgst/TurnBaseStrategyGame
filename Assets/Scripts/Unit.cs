using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    #region Private Variables

    private GridPosition _gridPosition;
    private MoveAction _moveAction;

    #endregion

    #region Cached

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtPosition(_gridPosition, this);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }

    #endregion

    #region Events

    #endregion

    #region Functions

    public MoveAction GetMoveAction()
    {
        return _moveAction;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    #endregion
}