
using UnityEngine;
using UnityEngine.Audio;

public abstract class TurretBase : MonoBehaviour
{
    [Header("Base Fields")]
    [SerializeField] private string _turretName;
    [SerializeField] private TurretType _turretType;
    [SerializeField] private AudioMixerGroup _mixerGroup;
    [SerializeField] protected GameObject _projectilePrefab;
    [SerializeField] protected GameObject _audioSourcePrefab;
    [SerializeField] protected AudioClip _firingSound;
    [Range(0f, 1f)][SerializeField] protected float _soundVolume = 1f;
    [Range(0f, 2f)][SerializeField] protected float _lowerPitchRange = 1f;
    [Range(0f, 2f)][SerializeField] protected float _higherPitchRange = 1f;

    public TurretType TurretType { get => _turretType; }
    public string TurretName { get => _turretName; }

    protected TurretFiringController turretFiringController;

    public virtual void StopTurret() { }

    protected virtual void Fire() { }
    protected virtual void FireProjectile(Transform firingPoint, float damagePerShot, float projectileForce)
    {
        GameObject projectile = Instantiate(
            _projectilePrefab,
            firingPoint.position,
            firingPoint.rotation);

        GameObject audioSourceObject = Instantiate(_audioSourcePrefab, firingPoint.position, firingPoint.rotation);
        AudioSource audioSource = audioSourceObject.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = _mixerGroup;
        audioSource.volume = _soundVolume;
        audioSource.pitch = Random.Range(_lowerPitchRange, _higherPitchRange);
        audioSource.PlayOneShot(_firingSound);
        audioSourceObject.GetComponent<SelfDestructAudioSource>().DestroySelf(_firingSound.length);

        Projectile projectileInfo = projectile.GetComponent<Projectile>();
        projectileInfo.Damage = damagePerShot;
        projectileInfo.Owner = turretFiringController.PlayerInput.playerIndex;

        projectile.GetComponent<Rigidbody>()
            .AddRelativeForce(Vector3.forward * projectileForce, ForceMode.Impulse);
    }
}

public enum TurretType
{
    Hold,
    Charge,
    Tap
}
