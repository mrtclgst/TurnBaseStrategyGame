using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables

    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainer;
    [SerializeField] private TextMeshProUGUI _actionPointsText;

    private List<ActionButtonUI> _actionButtonUIs = new();

    #endregion

    #region Cached

    #endregion

    #region Unity Methods

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionTriggered += UnitActionSystem_OnActionTriggered;
        Unit.OnAnyActionPointsChanged += Unit_OnEventTurnChanged;
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    #endregion

    #region Events

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs args)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionTriggered(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnEventTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
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
            _actionButtonUIs.Add(actionButtonUI);
        }
    }

    private void DeleteAllUnitActionButtons()
    {
        foreach (Transform buttonTransform in _actionButtonContainer)
        {
            Destroy(buttonTransform.gameObject);
        }

        _actionButtonUIs.Clear();
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI button in _actionButtonUIs)
        {
            button.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        _actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
    }

    #endregion
}