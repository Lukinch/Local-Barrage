using UnityEngine;

[CreateAssetMenu(fileName = "TurretTapFireStats", menuName = "Scriptable Objects/Turret Stats/Turret Tap Fire Stats")]
public class TurretTapFireStatsSO : ScriptableObject
{
    [SerializeField, Multiline] private string statsDescription;
    
    [Space(10)]
    [Tooltip("Pause time between each firing point Fire action")]
    public float timeBetweenShots = 0.2f;

    [Tooltip("Time till the next Fire action is available")]
    public float reloadTime = 0.3f;

    [Tooltip("Damage for each individual projectile")]
    [Range(0, 500)] public int damagePerShot;

    [Tooltip("Force added to each projectile when fired")]
    [Range(0, 200)] public int projectileForce;

    // TODO: Move this stat to the individual projectile instead
    [Tooltip("Damage type for the projectile")]
    public GlobalEnums.DamageType damageType;
}
