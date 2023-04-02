using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }

    public event EventHandler<OnShootEventArgs> OnEventShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit ShootingUnit;
    }

    [SerializeField] private int _maxShootDistance = 7;
    private State _state;
    private float _stateTimer;
    private Unit _targetUnit;
    private bool _canShootBullet;
    [SerializeField] private float _rotateSpeed = 1f;

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        _stateTimer -= Time.deltaTime;

        switch (_state)
        {
            case State.Aiming:
                Vector3 aimDirection = _targetUnit.GetWorldPosition() - transform.position;
                aimDirection.y = 0;
                Quaternion rotation = Quaternion.LookRotation(aimDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
                break;
            case State.Shooting:
                if (_canShootBullet)
                {
                    Shoot();
                    _canShootBullet = false;
                }

                break;
            case State.Cooloff:
                break;
        }

        if (_stateTimer <= 0)
        {
            NextState();
        }
    }

    private void Shoot()
    {
        OnEventShoot?.Invoke(this, new OnShootEventArgs()
        {
            ShootingUnit = this._unit,
            targetUnit = _targetUnit
        });
        _targetUnit.TakeDamage(40);
    }

    private void NextState()
    {
        switch (_state)
        {
            case State.Aiming:
                _state = State.Shooting;
                float shootingStateTime = .1f;
                _stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                _state = State.Cooloff;
                float cooloffStateTime = 0.5f;
                _stateTimer = cooloffStateTime;
                break;
            case State.Cooloff:
                //Before Refactor
                // _isActive = false;
                // _onActionComplete();

                //After refactor
                ActionComplete();

                break;
        }
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        //We are caching what will we shoot unit.
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        //We are setting up for beginning
        _state = State.Aiming;
        float aimingStateTime = .8f;
        _stateTimer = aimingStateTime;

        _canShootBullet = true;

        //Before Refactor
        // this._onActionComplete = onActionComplete;
        // _isActive = true;

        //After refactor
        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new();

        GridPosition unitGridPosition = _unit.GetGridPosition();
        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
        {
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                //for distance calculation
                int testDistance = Math.Abs(x) + Mathf.Abs(z);
                if (testDistance > _maxShootDistance)
                {
                    continue;
                }

                //Grid position is empty
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == _unit.IsEnemy())
                {
                    //Both units are on the same team so skipped.
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public Unit GetTargetUnit()
    {
        return _targetUnit;
    }

    public int GetMaxShootDistance()
    {
        return _maxShootDistance;
    }
}