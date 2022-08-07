
using System;
using UnityEngine;

public class PlayerHullCollision : MonoBehaviour, IDamageCollision
{
    [SerializeField] private UnityEngine.InputSystem.PlayerInput _owner;

    public Action<float, int> HullProjectileCollisionEvent;
    public Action<float> HullPickableCollisionEvent;

    public int OwnerInputId { get => _owner.playerIndex; }

    public void TakeProjectileDamage(float damage, int projectileOwner)
    {
        HullProjectileCollisionEvent?.Invoke(damage, projectileOwner);
    }

    public void TakePickableDamage(float damage)
    {
        HullPickableCollisionEvent?.Invoke(damage);
    }
}
