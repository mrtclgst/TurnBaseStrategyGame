using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private Transform _grenadeProjectilePrefab;
    [SerializeField] private int _maxThrowDistance = 5;

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeProjectileTransform =
            Instantiate(_grenadeProjectilePrefab, _unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviourComplete);
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new();

        GridPosition unitGridPosition = _unit.GetGridPosition();
        for (int x = -_maxThrowDistance; x <= _maxThrowDistance; x++)
        {
            for (int z = -_maxThrowDistance; z <= _maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                //for distance calculation
                int testDistance = Math.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxThrowDistance)
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction()
        {
            _gridPosition = gridPosition,
            _actionValue = 0,
        };
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionComplete();
    }
}