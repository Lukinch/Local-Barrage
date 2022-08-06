using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPoints : MonoBehaviour
{
    [SerializeField] private int pointsPerKill = 25;
    [SerializeField] private PlayerInput playerInput;
    private int points;

    public int Points { get => points; }

    public void AddPoints()
    {
        points += pointsPerKill;
        if (points >= GlobalPlayersManager.Instance.MaxAmountOfPointsPerPlayer)
        {
            points = 50;
        }
    }
    public int GetPointsInt() => points;
    public string GetPointsString() => points.ToString();
}
