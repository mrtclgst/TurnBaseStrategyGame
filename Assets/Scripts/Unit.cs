using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///INFO
///->Usage of Unit script: 
///ENDINFO

public class Unit : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables

    private Vector3 targetPosition;

    #endregion

    #region Cached

    #endregion

    #region Unity Methods

    private void Update()
    {
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {

            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }

    #endregion

    #region Events

    #endregion

    #region Functions

    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    #endregion
}