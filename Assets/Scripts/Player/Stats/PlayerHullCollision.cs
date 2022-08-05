
using System;
using UnityEngine;

public class PlayerHullCollision : MonoBehaviour, IDamageCollision
{
    public Action<float, int> HullProjectileCollisionEvent;
    public Action<float> HullPickableCollisionEvent;

    [SerializeField] private UnityEngine.InputSystem.PlayerInput owner;
    public int OwnerInputId { get => owner.playerIndex; }

    public void TakeProjectileDamage(float damage, int projectileOwner)
    {
        HullProjectileCollisionEvent?.Invoke(damage, projectileOwner);
    }

    public void TakePickableDamage(float damage)
    {
        HullPickableCollisionEvent?.Invoke(damage);
    }
}
