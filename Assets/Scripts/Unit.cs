using System;
using System.Security.Cryptography;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;


    #region Private Variables

    private const int DEFAULT_ACTION_POINTS = 2;
    private GridPosition _gridPosition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private ShootAction _shootAction;
    private BaseAction[] _baseActionArray;
    [SerializeField] private bool _isEnemy;
    private int _actionPoints = DEFAULT_ACTION_POINTS;
    private HealthSystem _healthSystem;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _shootAction = GetComponent<ShootAction>();
        _baseActionArray = GetComponents<BaseAction>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtPosition(_gridPosition, this);
        TurnSystem.Instance.OnTurnChanged += OnEventTurnChanged;
        _healthSystem.OnEventDead += HealthSystem_OnEventDead;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            GridPosition oldGridPosition = _gridPosition;
            _gridPosition = newGridPosition;
            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    #endregion

    #region Events

    private void OnEventTurnChanged(object sender, EventArgs e)
    {
        if (_isEnemy && !TurnSystem.Instance.IsPlayerTurn()
            || !_isEnemy && TurnSystem.Instance.IsPlayerTurn())
        {
            _actionPoints = DEFAULT_ACTION_POINTS;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HealthSystem_OnEventDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtPosition(_gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    #region Functions

    public MoveAction GetMoveAction()
    {
        return _moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return _spinAction;
    }

    public ShootAction GetShootAction()
    {
        return _shootAction;
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return _baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction action)
    {
        if (CanSpendActionPointsToTakeAction(action))
        {
            SpendActionPoints(action.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction action)
    {
        if (_actionPoints >= action.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SpendActionPoints(int amount)
    {
        _actionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetActionPoints()
    {
        return _actionPoints;
    }

    public bool IsEnemy()
    {
        return _isEnemy;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public void TakeDamage(int damageAmount)
    {
        _healthSystem.TakeDamage(damageAmount);
    }

    public float GetNormalizedHealth()
    {
        return _healthSystem.GetNormalizedHealth();
    }

    #endregion
}