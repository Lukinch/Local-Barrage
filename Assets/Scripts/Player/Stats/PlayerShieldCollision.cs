
using System;
using UnityEngine;

public class PlayerShieldCollision : MonoBehaviour, IDamageCollision
{
    public Action<float, int> ShieldProjectileCollisionEvent;
    public Action<float> ShieldPickableCollisionEvent;

    [SerializeField] private UnityEngine.InputSystem.PlayerInput owner;
    public int OwnerInputId { get => owner.playerIndex; }

    public void TakeProjectileDamage(float damage, int projectileOwner)
    {
        ShieldProjectileCollisionEvent?.Invoke(damage, projectileOwner);
    }

    public void TakePickableDamage(float damage)
    {
        ShieldPickableCollisionEvent?.Invoke(damage);
    }
}
