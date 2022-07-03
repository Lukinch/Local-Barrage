using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCollision : MonoBehaviour
{
    public Action<Projectile> ProjectileCollisionEvent;

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile != null)
        {
            ProjectileCollisionEvent?.Invoke(projectile);
        }
    }
}
