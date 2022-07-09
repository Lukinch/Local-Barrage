using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHullCollision : MonoBehaviour
{
    public Action<Projectile> HullCollisionEvent;

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile != null)
        {
            HullCollisionEvent?.Invoke(projectile);
        }
    }
}
