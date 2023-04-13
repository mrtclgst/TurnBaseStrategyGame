using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Vector3 _targetPosition;
    [SerializeField] private float _moveSpeed = 15f;
    [SerializeField] private float _reachedTargetDistance = 0.2f;
    [SerializeField] private float _damageRadius = 4f;
    [SerializeField] private int _explosionDamage = 30;
    public static event EventHandler OnAnyGrenadeExploded;
    private Action _onGrenadeBehaviourComplete;

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;
        transform.position += _moveSpeed * Time.deltaTime * moveDir;
        if (Vector3.Distance(transform.position, _targetPosition) < _reachedTargetDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(_targetPosition, _damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.TakeDamage(_explosionDamage);
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            _onGrenadeBehaviourComplete();
            Destroy(gameObject);
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this._onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }
}