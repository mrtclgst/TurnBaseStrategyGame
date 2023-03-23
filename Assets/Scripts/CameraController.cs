using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///INFO
///->Usage of CameraController script: 
///ENDINFO

public class CameraController : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    #endregion

    #region Cached

    #endregion

    #region Unity Methods

    private void Update()
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
        transform.position += moveVector * _moveSpeed * Time.deltaTime;

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


    #endregion

    #region Events

    #endregion

    #region Functions

    #endregion
}