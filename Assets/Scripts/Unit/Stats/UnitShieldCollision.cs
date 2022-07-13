using System;
using UnityEngine;

public class UnitShieldCollision : UnitCollision
{
    public Action<Projectile> ShieldCollisionEvent;

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile != null && projectile.owner != owner)
        {
            ShieldCollisionEvent?.Invoke(projectile);
        }
    }
}
