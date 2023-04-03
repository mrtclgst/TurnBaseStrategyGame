using System;
using UnityEngine;
using TMPro;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _actionPointsText;
    [SerializeField] private Unit _unit;
    [SerializeField] private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        CameraManager.OnEventActionCamera += CameraManager_OnEventActionCamera;
        UpdateActionPointText();
    }

    private void CameraManager_OnEventActionCamera(object sender, CameraManager.OnActionCameraEventArgs e)
    {
        if (_canvas != null)
        {
            _canvas.enabled = e.m_Show;
        }
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointText();
    }

    private void UpdateActionPointText()
    {
        _actionPointsText.text = _unit.GetActionPoints().ToString();
    }
}