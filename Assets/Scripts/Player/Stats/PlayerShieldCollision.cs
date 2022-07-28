
using System;
using UnityEngine;

public class PlayerShieldCollision : MonoBehaviour, IDamageCollision
{
    public Action<float> ShieldCollisionEvent;
    
    [SerializeField] private UnityEngine.InputSystem.PlayerInput owner;
    public int OwnerInputId { get => owner.playerIndex; }

    public void TakeDamage(float damage)
    {
        ShieldCollisionEvent?.Invoke(damage);
    }
}
