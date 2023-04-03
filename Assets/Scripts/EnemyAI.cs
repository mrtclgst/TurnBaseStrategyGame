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
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        _state = State.Busy;
                    }
                    else
                    {
                        //No more enemies have actions that they can take, end enemy turn
                        TurnSystem.Instance.NextTurn();
                    }
                }

                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        _timer = .5f;
        _state = State.TakingTurn;
    }

    private bool TryTakeEnemyAIAction(Action OnEnemyAIActionComplete)
    {
        Debug.Log("take enemy action");
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAction(enemyUnit, OnEnemyAIActionComplete))
            {
                Debug.Log("true");
                return true;
            }
        }

        Debug.Log("false");
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();
        GridPosition actionGridPosition = enemyUnit.GetGridPosition();

        //Refactored version
        if (spinAction.IsValidActionGridPosition(actionGridPosition))
        {
            if (enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
            {
                spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
                Debug.Log("V2 true");
                return true;
            }
        }

        Debug.Log("V2 false");
        return false;
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