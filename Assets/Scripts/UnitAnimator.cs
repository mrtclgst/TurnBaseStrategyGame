using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _bulletProjectilePrefab;
    [SerializeField] private Transform _shootPointTransform;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnEventStartMoving += MoveAction_OnStartMoving;
            moveAction.OnEventStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnEventShoot += ShootAction_OnShoot;
        }
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        _animator.SetTrigger("Shoot");
        Transform bulletTransform =
            Instantiate(_bulletProjectilePrefab, _shootPointTransform.position, Quaternion.identity);
        BulletProjectile projectile = bulletTransform.GetComponent<BulletProjectile>();
        Vector3 targetUnitShootPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootPosition.y = _shootPointTransform.position.y;
        projectile.Setup(targetUnitShootPosition);
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        _animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        _animator.SetBool("IsWalking", false);
    }
}