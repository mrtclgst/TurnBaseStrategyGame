using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    #region Public Variables

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;

    public static UnitActionSystem Instance { get; private set; }

    #endregion

    #region Private Variables

    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;
    private bool _isBusy = false;
    private BaseAction _selectedAction;

    #endregion

    #region Cached

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitActionSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(_selectedUnit);
    }

    private void Update()
    {
        if (_isBusy)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleUnitSelection())
        {
            return;
        }

        HandleSelectedAction();
    }

    #endregion

    #region Events

    #endregion

    #region Functions

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            //Refactored version
            if (_selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }

            // Old version
            //switch (_selectedAction)
            //{
            //    case MoveAction moveAction:
            //        if (_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            //        {
            //            SetBusy();
            //            moveAction.Move(mouseGridPosition, ClearBusy);
            //        }
            //        break;
            //    case SpinAction spinAction:
            //        SetBusy();
            //        spinAction.Spin(ClearBusy);
            //        break;
            //}
        }
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _unitLayerMask))
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == _selectedUnit)
                    {
                        //unit already selected
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private void SetBusy()
    {
        _isBusy = true;
    }

    private void ClearBusy()
    {
        _isBusy = false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction action)
    {
        _selectedAction = action;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return _selectedAction;
    }

    #endregion
}