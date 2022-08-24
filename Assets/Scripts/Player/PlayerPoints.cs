using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPoints : MonoBehaviour
{
    [SerializeField] private int _pointsPerWin = 25;
    [SerializeField] private PlayerInput _playerInput;
    private int _points;

    public int Points { get => _points; }

    public void AddPoints()
    {
        _points += _pointsPerWin;
        if (_points >= GlobalPlayersManager.Instance.MaxAmountOfPointsPerPlayer)
        {
            _points = 50;
        }
    }
    public int GetPointsInt() => _points;
    public string GetPointsString() => _points.ToString();
}
