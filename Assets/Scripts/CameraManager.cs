using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public class OnActionCameraEventArgs : EventArgs
    {
        public bool m_Show;
    }

    public static event EventHandler<OnActionCameraEventArgs> OnEventActionCamera;

    [SerializeField] private GameObject _actionCameraGameObject;
    [SerializeField] private GameObject _uiCanvasGameObject;

    private void Start()
    {
        BaseAction.OnEventAnyActionStarted += BaseAction_OnOnEventAnyActionStarted;
        BaseAction.OnEventAnyActionCompleted += BaseAction_OnOnEventAnyActionCompleted;
        HideActionCamera();
    }

    private void BaseAction_OnOnEventAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    private void BaseAction_OnOnEventAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;
                Vector3 actionCameraPosition =
                    shooterUnit.GetWorldPosition() +
                    cameraCharacterHeight +
                    shoulderOffset +
                    (shootDir * -1);
                _actionCameraGameObject.transform.position = actionCameraPosition;
                _actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                ShowActionCamera();
                break;
        }
    }

    private void ShowActionCamera()
    {
        OnEventActionCamera?.Invoke(this, new OnActionCameraEventArgs()
        {
            m_Show = true
        });
        _actionCameraGameObject.SetActive(true);
        _uiCanvasGameObject.SetActive(false);
    }

    private void HideActionCamera()
    {
        OnEventActionCamera?.Invoke(this, new OnActionCameraEventArgs()
        {
            m_Show = false
        });
        _actionCameraGameObject.SetActive(false);
        _uiCanvasGameObject.SetActive(true);
    }
}