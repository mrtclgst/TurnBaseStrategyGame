using System;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isOpen;
    private Animator _animator;
    private GridPosition _gridPosition;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    private Action _onInteractComplete;
    private float _timer;
    private bool _isActive;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _isActive = false;
            _onInteractComplete();
        }
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(_gridPosition, this);
        if (_isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void Interact(Action onInteractComplete)
    {
        _isActive = true;
        _onInteractComplete = onInteractComplete;
        _timer = 0.5f;
        if (_isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        _isOpen = true;
        _animator.SetBool(IsOpen, _isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, true);
    }

    public void CloseDoor()
    {
        _isOpen = false;
        _animator.SetBool(IsOpen, _isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, false);
    }
}