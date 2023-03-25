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
        _buttonText.text = action.GetActionName();
        _button.onClick.AddListener(() => UnitActionSystem.Instance.SetSelectedAction(action));
    }

    #endregion
}