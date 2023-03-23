using UnityEngine;

public class Unit : MonoBehaviour
{
    #region Private Variables

    [SerializeField] private Animator _unitAnimator;
    [SerializeField] float _rotateSpeed = 10f;
    [SerializeField] float _moveSpeed = 4f;
    private Vector3 _targetPosition;
    private GridPosition _gridPosition;

    #endregion

    #region Cached

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtPosition(_gridPosition, this);
    }

    private void Update()
    {
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += _moveSpeed * Time.deltaTime * moveDirection;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
            _unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            _unitAnimator.SetBool("IsWalking", false);
        }

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
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