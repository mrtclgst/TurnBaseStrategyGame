using System;
using JetBrains.Annotations;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private MeshRenderer _meshRenderer;
    private Action _onInteractComplete;
    private bool _isGreen;
    private bool _isActive;
    private float _timer;
    private GridPosition _gridPosition;

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(_gridPosition, this);
        SetColorGreen();
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _isActive = false;
            _onInteractComplete();
        }
    }

    private void SetColorGreen()
    {
        _isGreen = true;
        _meshRenderer.material = _greenMaterial;
    }

    private void SetColorRed()
    {
        _isGreen = false;
        _meshRenderer.material = _redMaterial;
    }

    public void Interact(Action onInteractComplete)
    {
        _onInteractComplete = onInteractComplete;
        _isActive = true;
        _timer = .5f;
        if (_isGreen)
        {
            SetColorRed();
        }
        else
        {
            SetColorGreen();
        }
    }
}