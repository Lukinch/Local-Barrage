using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShieldCollision : MonoBehaviour
{
    public Action<Projectile> ShieldCollisionEvent;

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile != null)
        {
            ShieldCollisionEvent?.Invoke(projectile);
        }
    }
}
