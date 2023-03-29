using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnEventAnyActionStarted;
    public static event EventHandler OnEventAnyActionCompleted;

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

    protected void ActionStart(Action onActionComplete)
    {
        _isActive = true;
        this._onActionComplete = onActionComplete;
        OnEventAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionComplete()
    {
        _isActive = false;
        _onActionComplete();
        OnEventAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    public Unit GetUnit()
    {
        return _unit;
    }
}