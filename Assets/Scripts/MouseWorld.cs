using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

///INFO
///->Usage of MouseWorld script: 
///ENDINFO

public class MouseWorld : MonoBehaviour
{
    #region Public Variables


    #endregion

    #region Static Variables

    [SerializeField] private static MouseWorld instance;

    #endregion

    #region Private Variables

    [SerializeField] private LayerMask _mousePlaneLayerMask;


    #endregion

    #region Cached

    #endregion

    #region Unity Methods

    private void Awake()
    {
        instance = this;
    }

    #endregion

    #region Events

    #endregion

    #region Functions

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, instance._mousePlaneLayerMask);
        return hit.point;
    }

    #endregion
}