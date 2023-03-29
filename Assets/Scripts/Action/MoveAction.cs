using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnEventStartMoving;
    public event EventHandler OnEventStopMoving;

    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private int _maxMovementDistance = 4;
    private Vector3 _targetPosition;

    protected override void Awake()
    {
        base.Awake();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            var lookPos = this._targetPosition - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.position += _moveSpeed * Time.deltaTime * moveDirection;
            // transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
        }
        else
        {
            OnEventStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new();

        GridPosition unitGridPosition = _unit.GetGridPosition();
        for (int x = -_maxMovementDistance; x <= _maxMovementDistance; x++)
        {
            for (int z = -_maxMovementDistance;
                 z <= _maxMovementDistance;
                 z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this._targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        OnEventStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "Move";
    }
}