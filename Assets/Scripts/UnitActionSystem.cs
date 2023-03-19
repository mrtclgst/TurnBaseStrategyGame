using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///INFO
///->Usage of UnitActionSystem script: 
///ENDINFO

public class UnitActionSystem : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables

    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;

    #endregion

    #region Cached


    #endregion

    #region Unity Methods


    #endregion

    #region Events
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
                    _selectedUnit = unit;
                    return true;
                }
            }
        }

        return false;
    }

    #endregion
}