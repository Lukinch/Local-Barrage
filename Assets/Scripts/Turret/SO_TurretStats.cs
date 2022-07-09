using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Stats", menuName = "Scriptable Objects/Turret Stats")]
public class SO_TurretStats : ScriptableObject
{
    [Range(0, 500)] public int damagePerShot;
    [Range(0, 200)] public int projectileForce;
    public GlobalEnums.DamageType damageType;
}
