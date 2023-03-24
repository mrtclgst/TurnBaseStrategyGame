using System;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables

    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _zoomAmount = 1f;
    [SerializeField] private float _zoomSpeed = 10f;
    private Vector3 _targetFollowOffset;
    private CinemachineTransposer _cinemachineTransposer;

    #endregion

    #region Cached

    #endregion

    #region Unity Methods

    private void Start()
    {
        _cinemachineTransposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    #endregion

    #region Events

    #endregion

    #region Functions

    private void HandleMovement()
    {
        Vector3 inputMoveDir = new();
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = 1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += _moveSpeed * Time.deltaTime * moveVector;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new();
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }

        transform.eulerAngles += _rotationSpeed * Time.deltaTime * rotationVector;
    }

    private void HandleZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            _targetFollowOffset.y -= _zoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            _targetFollowOffset.y += _zoomAmount;
        }

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        _cinemachineTransposer.m_FollowOffset =
            Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset, Time.deltaTime * _zoomSpeed);
    }

    #endregion
}