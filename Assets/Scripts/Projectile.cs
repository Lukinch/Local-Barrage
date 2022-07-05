using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactParticles;

    public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        Instantiate(impactParticles, transform.position, transform.rotation);
    }
}
