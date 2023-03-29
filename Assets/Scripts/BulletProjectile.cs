using System;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 _targetPosition;
    [SerializeField] private float _bulletSpeed = 200f;
    [SerializeField] private TrailRenderer _projectileTrail;
    [SerializeField] private Transform _bulletHitVFX;

    public void Setup(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);

        transform.position += _bulletSpeed * Time.deltaTime * moveDir;

        float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            Instantiate(_bulletHitVFX, _targetPosition, Quaternion.identity);
            transform.position = _targetPosition;
            _projectileTrail.transform.parent = null;
            Destroy(gameObject);
        }
    }
}