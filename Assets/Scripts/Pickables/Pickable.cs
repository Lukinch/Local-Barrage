using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Pickable : MonoBehaviour
{
    [SerializeField] protected AudioClip pickupSfx;
    [SerializeField] protected ParticleSystem pickupVfx;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPickableCollision player = other.GetComponent<PlayerPickableCollision>();

        if (player != null)
        {
            ImplementEffect(player);
        }
    }

    protected virtual void OnPicked()
    {
        if (pickupVfx)
        {
            Instantiate(pickupVfx, transform.position, Quaternion.identity);
        }

        if (pickupSfx)
        {
            CreateSFX(pickupSfx, transform.position, 3f, 0f);
            StartCoroutine(nameof(WaitForSfxEnd), pickupSfx.length);
        }
        else
        {
            NotifyDestruction();
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitForSfxEnd(float seconds)
    {
        yield return new WaitForSeconds(seconds - 0.1f);
        NotifyDestruction();
    }

    protected virtual void CreateSFX(AudioClip clip, Vector3 position, float spatialBlend, float rolloffDistanceMin = 1f)
    {
        GameObject impactSfxInstance = new GameObject();
        impactSfxInstance.transform.position = position;
        AudioSource source = impactSfxInstance.AddComponent<AudioSource>();
        source.clip = clip;
        source.spatialBlend = spatialBlend;
        source.minDistance = rolloffDistanceMin;
        source.Play();

        Destroy(gameObject, clip.length);
    }

    protected abstract void ImplementEffect(PlayerPickableCollision player);
    protected abstract void NotifyDestruction();
}
