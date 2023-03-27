using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        _totalSpinAmount += spinAddAmount;
        if (_totalSpinAmount >= 360)
        {
            _isActive = false;
            _onActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
    {
        _onActionComplete = onSpinComplete;
        _isActive = true;
        _totalSpinAmount = 0;
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = _unit.GetGridPosition();
        return new List<GridPosition> { unitGridPosition };
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }
}