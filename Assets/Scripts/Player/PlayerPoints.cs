using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPoints : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    private float _points;

    public float Points { get => _points; }

    public void AddPoints(float amount)
    {
        _points += amount;
        float maxPoints = GlobalPlayersManager.Instance.MaxAmountOfPointsToWin;
        if (_points >= maxPoints)
        {
            _points = maxPoints;
        }
    }
    public float GetPointsInt() => _points;
    public string GetPointsString() => _points.ToString();
}
