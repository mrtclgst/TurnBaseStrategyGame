using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float _timer = 0;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += OnEventTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        _timer -= Time.deltaTime;
        if (_timer >= 0f)
        {
            TurnSystem.Instance.NextTurn();
        }
    }

    private void OnEventTurnChanged(object sender, EventArgs e)
    {
        _timer = 2f;
    }
}