using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretFiringController : MonoBehaviour
{
    public PlayerInput Owner;

    public event Action onFireInstantPerformed;
    
    public event Action onFireHoldStarted;
    public event Action onFireHoldCanceled;
    public event Action onFireHoldPerformed;

    public event Action onFireChargeStarted;
    public event Action onFireChargeCanceled;
    public event Action onFireChargePerformed;

    public void OnFireInstant(InputAction.CallbackContext context)
    {
        if (context.performed) onFireInstantPerformed?.Invoke();
    }
    public void OnFireHold(InputAction.CallbackContext context)
    {
        if (context.started) onFireHoldStarted?.Invoke();
        if (context.canceled) onFireHoldCanceled?.Invoke();
        if (context.performed) onFireHoldPerformed?.Invoke();
    }
    public void OnFireCharge(InputAction.CallbackContext context)
    {
        if (context.started) onFireChargeStarted?.Invoke();
        if (context.canceled) onFireChargeCanceled?.Invoke();
        if (context.performed) onFireChargePerformed?.Invoke();
    }
}