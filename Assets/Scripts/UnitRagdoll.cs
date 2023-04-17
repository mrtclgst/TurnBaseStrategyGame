using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform _ragdollRootBone;
    [SerializeField] private Transform _weapon;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransform(originalRootBone, _ragdollRootBone);
        ApplyRagdollToWeapon();

        Vector3 explosionPosition = GetDirectionToRagdollForceFromOtherUnit();
        ApplyExplosionToRagdoll(_ragdollRootBone, 500f, explosionPosition, 10f);
    }

    private Vector3 GetDirectionToRagdollForceFromOtherUnit()
    {
        float offset = 0.5f;
        Vector3 explosionPosition =
            (((UnitActionSystem.Instance.GetSelectedUnit().GetWorldPosition() - transform.position).normalized) *
             offset) + transform.position;
        return explosionPosition;
    }

    private void MatchAllChildTransform(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild)
            {
                cloneChild.SetPositionAndRotation(child.position, child.rotation);

                MatchAllChildTransform(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition,
        float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }

    private void ApplyRagdollToWeapon()
    {
        _weapon.parent = null;
        Rigidbody weaponRB = _weapon.gameObject.AddComponent<Rigidbody>();
        weaponRB.mass = 4f;
    }
}