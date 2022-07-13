using System;
using UnityEngine;

public class UnitHullCollision : UnitCollision
{
    public Action<Projectile> HullCollisionEvent;

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile != null && projectile.owner != owner)
        {
            HullCollisionEvent?.Invoke(projectile);
        }
    }
}
