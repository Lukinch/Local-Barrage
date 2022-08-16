
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _projectileObject;
    [SerializeField] private ParticleSystem _impactParticles;
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
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        _impactParticles.Play();
        _projectileObject.SetActive(false);
        StartCoroutine(DestroyGameObject());
    }

    private IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(_impactParticles.main.duration);
        DestroySelf();
    }

    public void DestroySelf()
    {
        // TODO: Implement object pooling
        Destroy(gameObject);
    }
}
