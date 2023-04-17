using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private TextMeshProUGUI _turnNumberText;
    [SerializeField] private GameObject _enemyTurnVisualGO;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += OnEventTurnChanged;
        _endTurnButton.onClick.AddListener(() => TurnSystem.Instance.NextTurn());
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnVisibility();
    }

    #region Events

    private void OnEventTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnVisibility();
    }

    #endregion

    #region Methods

    private void UpdateTurnText()
    {
        _turnNumberText.text = "Turn " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual()
    {
        _enemyTurnVisualGO.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnVisibility()
    {
        _endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

    #endregion
}