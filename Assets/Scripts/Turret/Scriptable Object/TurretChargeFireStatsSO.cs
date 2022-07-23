using UnityEngine;

[CreateAssetMenu(fileName = "TurretChargeFireStats", menuName = "Scriptable Objects/Turret Stats/Turret Charge Fire Stats")]
public class TurretChargeFireStatsSO : ScriptableObject
{
    [SerializeField, Multiline] private string statsDescription;
    
    [Space(10)]
    [Tooltip("Pause time between each firing point Fire action")]
    public float timeBetweenShots = 0.2f;
    [Tooltip("Time that will take to charge to fire a projectile")]
    [Range(0.1f, 5)] public float chargeTime = 1.5f;

    [Tooltip("Damage for each individual projectile")]
    [Range(0, 500)] public int damagePerShot;

    [Tooltip("Force added to each projectile when fired")]
    [Range(0, 200)] public int projectileForce;

    // TODO: Move this stat to the individual projectile instead
    [Tooltip("Damage type for the projectile")]
    public GlobalEnums.DamageType damageType;
}