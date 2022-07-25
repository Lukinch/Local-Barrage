using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickableCollision : MonoBehaviour
{
    [SerializeField] private PlayerTurretController playerTurretController;
    [SerializeField] private PlayerDamageController playerDamageController;

    public PlayerTurretController TurretController
    {
        get => playerTurretController;
    }

    public PlayerDamageController PlayerDamageController
    {
        get => playerDamageController;
    }
}
