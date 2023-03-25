using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///INFO
///->Usage of UnitActionSystemUI script: 
///ENDINFO

public class UnitActionSystemUI : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables

    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainer;

    #endregion

    #region Cached

    #endregion

    #region Unity Methods

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        CreateUnitActionButtons();
    }

    #endregion

    #region Events

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs args)
    {
        CreateUnitActionButtons();
    }

    #endregion

    #region Functions

    private void CreateUnitActionButtons()
    {
        DeleteAllUnitActionButtons();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainer);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetButtonAction(baseAction);
        }
    }

    private void DeleteAllUnitActionButtons()
    {
        foreach (Transform buttonTransform in _actionButtonContainer)
        {
            Destroy(buttonTransform.gameObject);
        }
    }


    #endregion
}