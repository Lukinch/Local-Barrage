
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject _impactParticles;
    [SerializeField] private int _timeToDestroy = 3;
    private int _currentTime;

    [HideInInspector] public int Owner;
    [HideInInspector] public float Damage;

    private void Start()
    {
        _currentTime = _timeToDestroy;
        StartCoroutine(nameof(DeathCountDown));
    }

    private IEnumerator DeathCountDown()
    {
        while (_currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            _currentTime--;
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

            unitCollision.TakeProjectileDamage(Damage, Owner);

            Destroy();
        }
        else
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        Instantiate(_impactParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
