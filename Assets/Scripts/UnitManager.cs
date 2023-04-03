using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///INFO
///->Usage of UnitManager script: 
///ENDINFO

public class UnitManager : MonoBehaviour
{
    #region Public Variables

    public static UnitManager Instance { get; private set; }

    #endregion

    #region Private Variables

    private List<Unit> _unitList;
    private List<Unit> _friendlyUnitList;
    private List<Unit> _enemyUnitList;

    #endregion

    #region Cached

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitManager" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _unitList = new List<Unit>();
        _friendlyUnitList = new List<Unit>();
        _enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    #endregion

    #region Events

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = (Unit)sender;
        if (unit == null) return;

        _unitList.Add(unit);
        if (unit.IsEnemy())
        {
            _enemyUnitList.Add(unit);
        }
        else
        {
            _friendlyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = (Unit)sender;
        if (unit == null) return;

        _unitList.Remove(unit);
        if (unit.IsEnemy())
        {
            _enemyUnitList.Remove(unit);
        }
        else
        {
            _friendlyUnitList.Remove(unit);
        }
    }

    #endregion

    #region Functions

    public List<Unit> GetUnitList()
    {
        return _unitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return _enemyUnitList;
    }

    public List<Unit> GetFriendUnitList()
    {
        return _friendlyUnitList;
    }

    #endregion
}