using System;
using UnityEngine;


///INFO
///->Usage of UnitSelectedVisual script: 
///ENDINFO
public class UnitSelectedVisual : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables

    [SerializeField] private Unit _unit;

    private MeshRenderer _meshRenderer;

    #endregion

    #region Cached

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UpdateVisual();
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }

    #endregion

    #region Events

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        _meshRenderer.enabled = UnitActionSystem.Instance.GetSelectedUnit() == _unit;
    }

    #endregion

    #region Functions

    #endregion
}