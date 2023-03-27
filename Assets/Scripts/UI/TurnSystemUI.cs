using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private TextMeshProUGUI _turnNumberText;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += OnEventTurnChanged;
        _endTurnButton.onClick.AddListener(() => TurnSystem.Instance.NextTurn());
        UpdateTurnText();
    }

    #region Events

    private void OnEventTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
    }

    #endregion

    #region Methods

    private void UpdateTurnText()
    {
        _turnNumberText.text = "Turn " + TurnSystem.Instance.GetTurnNumber();
    }

    #endregion
}