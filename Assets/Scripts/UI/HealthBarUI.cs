using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private Image _healthBarFillImage;
    [SerializeField] private Canvas _canvas;

    private void Start()
    {
        CameraManager.OnEventActionCamera += CameraManager_OnEventActionCamera;
        _healthSystem.OnEventDamaged += HealthSystem_OnEventDamaged;
        UpdateHealthBar();
    }

    private void CameraManager_OnEventActionCamera(object sender, CameraManager.OnActionCameraEventArgs e)
    {
        _canvas.enabled = e.m_Show;
    }

    private void UpdateHealthBar()
    {
        _healthBarFillImage.fillAmount = _healthSystem.GetNormalizedHealth();
    }

    private void HealthSystem_OnEventDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}