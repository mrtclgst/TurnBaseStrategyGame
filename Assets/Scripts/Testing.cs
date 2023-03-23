using System;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private GridSystem _gridSystem;

    private void Start()
    {
        _gridSystem = new GridSystem(5, 5, 2f);
    }

    private void Update()
    {
        Debug.Log(_gridSystem.GetGridPosition(MouseWorld.GetPosition()));
    }
}