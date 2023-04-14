using System;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Vector3 _targetPosition;
    [SerializeField] private Transform _grenadeExplosionVFX;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private AnimationCurve _arcYAnimationCurve;
    [SerializeField] private float _moveSpeed = 15f;
    [SerializeField] private float _reachedTargetDistance = 0.2f;
    [SerializeField] private float _damageRadius = 4f;
    [SerializeField] private int _explosionDamage = 30;
    public static event EventHandler OnAnyGrenadeExploded;
    private Action _onGrenadeBehaviourComplete;
    private float _totalDistance;
    private Vector3 _positionXZ;

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - _positionXZ).normalized;
        _positionXZ += _moveSpeed * Time.deltaTime * moveDir;
        float _distance = Vector3.Distance(_positionXZ, _targetPosition);
        float distanceNormalized = 1 - _distance / _totalDistance;
        float positionY = _arcYAnimationCurve.Evaluate(distanceNormalized);
        transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);

        if (Vector3.Distance(_positionXZ, _targetPosition) < _reachedTargetDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(_targetPosition, _damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.TakeDamage(_explosionDamage);
                }

                if (collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damage();
                }
            }

            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            _trailRenderer.transform.parent = null;
            Instantiate(_grenadeExplosionVFX, _targetPosition + Vector3.up * 1, Quaternion.identity);
            _onGrenadeBehaviourComplete();
            Destroy(gameObject);
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviourComplete)
    {
        this._onGrenadeBehaviourComplete = onGrenadeBehaviourComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        _positionXZ = transform.position;
        _positionXZ.y = 0;
        _totalDistance = Vector3.Distance(_positionXZ, _targetPosition);
    }
}