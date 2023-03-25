using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit _unit;
    protected bool _isActive = false;
    protected Action _onActionComplete;

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    //herkesi bunu implemente etmek icin zorluyoruz abstract ile 
    public abstract string GetActionName();
}