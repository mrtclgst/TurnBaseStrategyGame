using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit _unit;
    protected bool _isActive = false;
    protected Action _onActionComplete;

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    //herkesi bunu implemente etmek icin zorluyoruz abstract ile 
    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }
    public abstract List<GridPosition> GetValidActionGridPositionList();

}