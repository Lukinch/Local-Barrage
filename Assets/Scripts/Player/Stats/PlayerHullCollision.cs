
using System;
using UnityEngine;

public class PlayerHullCollision : MonoBehaviour, IDamageCollision
{
    public Action<float> HullCollisionEvent;
    
    [SerializeField] private UnityEngine.InputSystem.PlayerInput owner;
    public int OwnerInputId { get => owner.playerIndex; }

    public void TakeDamage(float damage)
    {
        HullCollisionEvent?.Invoke(damage);
    }
}
