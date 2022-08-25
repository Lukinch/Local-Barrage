
using System;
using UnityEngine;

public class DummyHullCollider : MonoBehaviour, IDamageCollision
{
    public Action<float, int> HullProjectileCollisionEvent;
    public Action<float> HullPickableCollisionEvent;

    public int OwnerInputId { get => -1; }

    public void TakeProjectileDamage(float damage, int projectileOwner)
    {
        HullProjectileCollisionEvent?.Invoke(damage, projectileOwner);
    }

    public void TakePickableDamage(float damage)
    {
        HullPickableCollisionEvent?.Invoke(damage);
    }
}
