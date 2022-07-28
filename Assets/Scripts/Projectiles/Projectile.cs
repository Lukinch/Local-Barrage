
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactParticles;
    [SerializeField] private int timeToDestroy = 3;
    private int currentTime;

    [HideInInspector] public int Owner;
    [HideInInspector] public float Damage;

    private void Start()
    {
        currentTime = timeToDestroy;
        StartCoroutine(nameof(DeathCountDown));
    }

    private IEnumerator DeathCountDown()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
        }
        DestroySelf();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageCollision unitCollision = other.GetComponent<IDamageCollision>();

        if (unitCollision != null)
        {
            if (Owner == unitCollision.OwnerInputId) return;

            unitCollision.TakeDamage(Damage);

            Destroy();
        }
        else
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        Instantiate(impactParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
