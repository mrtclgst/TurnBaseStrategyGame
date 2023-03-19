using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///INFO
///->Usage of Unit script: 
///ENDINFO

public class Unit : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private Animator _unitAnimator;
    [SerializeField] float _rotateSpeed = 10f;
    [SerializeField] float _moveSpeed = 4f;
    private Vector3 _targetPosition;

    #endregion

    #region Cached

    #endregion

    #region Unity Methods
    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
            _unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            _unitAnimator.SetBool("IsWalking", false);
        }
    }

    #endregion

    #region Events

    #endregion

    #region Functions

    public void Move(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
    }

    #endregion
}