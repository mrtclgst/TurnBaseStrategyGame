using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///INFO
///->Usage of UnitActionSystem script: 
///ENDINFO

public class UnitActionSystem : MonoBehaviour
{
    #region Public Variables
    public event EventHandler OnSelectedUnitChanged;
    public static UnitActionSystem Instance { get; private set; }
    #endregion

    #region Private Variables

    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;

    #endregion

    #region Cached


    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitActionSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection())
            {
                return;
            }

            _selectedUnit.Move(MouseWorld.GetPosition());
        }
    }
    #endregion

    #region Events

    #endregion

    #region Functions

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _unitLayerMask))
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }

    #endregion
}