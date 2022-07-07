using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEnums
{
    public enum DamageType
    {
        /// <summary>100% Damage to Shields, no Damage to Hull</summary>
        THERMAL,
        /// <summary>30% Damage to Shields, 70% Damage to Hull</summary>
        KINETIC,
        /// <summary>50% Damage to Shields, 50% Damage to Hull</summary>
        THERMAL_KINETIC,
        /// <summary>no Damage to Shields, 100% Damage to Hull</summary>
        EXPLOSIVE
    }

    public enum FiringMode
    {
        /// <summary>One Tap per fire action, holding the key will only fire once</summary>
        TAP_TO_FIRE,
        /// <summary>Hold to continuously fire projectiles</summary>
        HOLD_TO_FIRE,
        /// <summary>Hold to charge and then fire one projectile</summary>
        HOLD_TO_CHARGE
    }
}
