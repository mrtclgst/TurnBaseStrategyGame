using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

///INFO
///->Usage of ActionButtonUI script: 
///ENDINFO

public class ActionButtonUI : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables

    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject _selectedBorder;

    private BaseAction _buttonAction;

    #endregion

    #region Cached

    #endregion

    #region Unity Methods



    #endregion

    #region Events

    #endregion

    #region Functions

    public void SetButtonAction(BaseAction action)
    {
        _buttonAction = action;
        _buttonText.text = action.GetActionName();
        _button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(action);
            UpdateSelectedVisual();
        }
        );
    }

    public void UpdateSelectedVisual()
    {
        BaseAction _selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        _selectedBorder.SetActive(_buttonAction == _selectedAction);
    }

    #endregion
}