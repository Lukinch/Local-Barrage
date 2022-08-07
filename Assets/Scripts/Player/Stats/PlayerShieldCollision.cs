
using System;
using UnityEngine;

public class PlayerShieldCollision : MonoBehaviour, IDamageCollision
{
    [SerializeField] private UnityEngine.InputSystem.PlayerInput _owner;

    public Action<float, int> ShieldProjectileCollisionEvent;
    public Action<float> ShieldPickableCollisionEvent;

    public int OwnerInputId { get => _owner.playerIndex; }

    public void TakeProjectileDamage(float damage, int projectileOwner)
    {
        ShieldProjectileCollisionEvent?.Invoke(damage, projectileOwner);
    }

    public void TakePickableDamage(float damage)
    {
        ShieldPickableCollisionEvent?.Invoke(damage);
    }
}
