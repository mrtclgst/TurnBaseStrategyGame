using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State _state;
    private float _timer = 0;
    private void Awake()
    {
        _state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += OnEventTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch (_state)
        {
            case State.WaitingForEnemyTurn:

                break;
            case State.TakingTurn:

                _state = State.Busy;
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    _state = State.Busy;
                    TakeEnemyAIAction(SetStateTakingTurn); 
                }

                break;
            case State.Busy:

                break;
            default:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        _timer = .5f;
        _state = State.TakingTurn;
    }

    private void TakeEnemyAIAction(Action OnEnemyAIActionComplete)
    {
        Debug.Log("take enemy action");
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            TakeEnemyAIAction(enemyUnit, OnEnemyAIActionComplete);
        }
    }

    private void TakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();
        GridPosition actionGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

        //Refactored version
        if (spinAction.IsValidActionGridPosition(actionGridPosition))
        {
            if (enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
            {
                spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
            }
        }
    }

    private void OnEventTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            _state = State.TakingTurn;
            _timer = 2f;
        }
    }
}