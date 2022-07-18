using UnityEngine;

[CreateAssetMenu(fileName = "TurretHoldFireStats", menuName = "Scriptable Objects/Turret Stats/Turret Hold Fire Stats")]
public class TurretHoldFireStatsSO : ScriptableObject
{
    [SerializeField, Multiline] private string statsDescription;
    
    [Space(10)]
    [Tooltip("Pause time between each firing point Fire action")]
    public float timeBetweenShots = 0.2f;
    [Tooltip("How often the Fire action should be called")]
    public float timeBetweenFireActions = 10;
    [Tooltip("Time till the turret overheats when hold to fire mode is selected")]
    public float overheatTime = 5f;
    [Tooltip("Time till the turret cools off when hold to fire mode is selected")]
    public float coolingTime = 3f;
    [Tooltip("Time that will take to charge to fire a projectile")]
    public float overheatPerShot = 0.3f;

    [Tooltip("Damage for each individual projectile")]
    [Range(0, 500)] public int damagePerShot;

    [Tooltip("Force added to each projectile when fired")]
    [Range(0, 200)] public int projectileForce;

    // TODO: Move this stat to the individual projectile instead
    [Tooltip("Damage type for the projectile")]
    public GlobalEnums.DamageType damageType;
}
