using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickable : MonoBehaviour
{
    [SerializeField] protected AudioClip pickupSfx;
    [SerializeField] protected ParticleSystem pickupVfx;

    protected virtual void OnPicked()
    {
        PlayPickupFeedback();
    }

    public void PlayPickupFeedback()
    {
        if (pickupSfx)
        {
            CreateSFX(pickupSfx, transform.position, 3f, 0f);
        }

        if (pickupVfx)
        {
            Instantiate(pickupVfx, transform.position, Quaternion.identity);
        }

        if (!pickupSfx) Destroy(gameObject);
    }

    private void CreateSFX(AudioClip clip, Vector3 position, float spatialBlend, float rolloffDistanceMin = 1f)
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
}
