using System;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform _ragdollPrefab;
    [SerializeField] private Transform _ragdollOriginalRootBone;
    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        _healthSystem.OnEventDead += HealthSystem_OnEventDead;
    }

    private void HealthSystem_OnEventDead(object sender, EventArgs e)
    {
        Transform ragdoll = Instantiate(_ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdoll.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(_ragdollOriginalRootBone);
    }
}