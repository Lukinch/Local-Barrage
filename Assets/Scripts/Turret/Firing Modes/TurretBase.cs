
using UnityEngine;

public abstract class TurretBase : MonoBehaviour
{
    [Header("Base Fields")]
    [SerializeField] private string _turretName;
    [SerializeField] private TurretType _turretType;
    [SerializeField] protected GameObject _projectilePrefab;

    public TurretType TurretType { get => _turretType; }
    public string TurretName { get => _turretName; }

    protected TurretFiringController turretFiringController;

    public virtual void StopTurret() {}

    protected virtual void Fire() { }
    protected virtual void FireProjectile(Transform firingPoint, float damagePerShot, float projectileForce)
    {
        GameObject projectile = Instantiate(
            _projectilePrefab,
            firingPoint.position,
            firingPoint.rotation);

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
