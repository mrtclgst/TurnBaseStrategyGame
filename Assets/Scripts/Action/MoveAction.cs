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
    private List<Vector3> _targetPositionList;
    private int _currentPositionIndex = 0;

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        Vector3 targetPosition = _targetPositionList[_currentPositionIndex];

        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            var lookPos = targetPosition - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.position += _moveSpeed * Time.deltaTime * moveDirection;
            // transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
        }
        else
        {
            _currentPositionIndex++;
            if (_currentPositionIndex >= _targetPositionList.Count)
            {
                OnEventStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
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

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }
   
                if (!Pathfinding.Instance.HasPath(_unit.GetGridPosition(), testGridPosition))
                {
                    continue;
                }

                //floattan kurtulmak icin boyle yapmistik o yuzden bu kismi da 10 ile carpiyoruz.
                int pathfindingDistanceMultiplier = 10;
                if (Pathfinding.Instance.GetPathLength(_unit.GetGridPosition(), testGridPosition) >
                    _maxMovementDistance * pathfindingDistanceMultiplier)
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
        List<GridPosition> pathGridPositionList =
            Pathfinding.Instance.FindPath(_unit.GetGridPosition(), gridPosition, out int pathLength);

        _currentPositionIndex = 0;
        _targetPositionList = new List<Vector3>();

        for (int index = 0; index < pathGridPositionList.Count; index++)
        {
            _targetPositionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPositionList[index]));
        }

        OnEventStartMoving?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = _unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction()
        {
            _gridPosition = gridPosition,
            _actionValue = targetCountAtGridPosition * 10,
        };
    }
}