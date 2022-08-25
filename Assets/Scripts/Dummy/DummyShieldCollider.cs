
using System;
using UnityEngine;

public class DummyShieldCollider : MonoBehaviour, IDamageCollision
{
    public Action<float, int> ShieldProjectileCollisionEvent;
    public Action<float> ShieldPickableCollisionEvent;

    public int OwnerInputId { get => -1; }

    public void TakeProjectileDamage(float damage, int projectileOwner)
    {
        ShieldProjectileCollisionEvent?.Invoke(damage, projectileOwner);
    }

    public void TakePickableDamage(float damage)
    {
        ShieldPickableCollisionEvent?.Invoke(damage);
    }
}
