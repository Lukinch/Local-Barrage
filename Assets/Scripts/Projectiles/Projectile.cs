
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactParticles;

    [HideInInspector] public GameObject owner;
    [HideInInspector] public float damage;

    private void OnTriggerEnter(Collider other)
    {
        UnitCollision unitCollision = other.GetComponent<UnitCollision>();

        if (unitCollision != null)
        {
            if (owner == unitCollision.owner) return;

            Instantiate(impactParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(impactParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
