using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretFiringController : MonoBehaviour
{
    public PlayerInput PlayerInput;

    public event Action OnFireInstantPerformed;
    
    public event Action OnFireHoldStarted;
    public event Action OnFireHoldCanceled;
    public event Action OnFireHoldPerformed;

    public event Action OnFireChargeStarted;
    public event Action OnFireChargeCanceled;
    public event Action OnFireChargePerformed;

    public bool ShouldTriggerEvents = true;

    public void OnFireInstant(InputAction.CallbackContext context)
    {
        if (!ShouldTriggerEvents) return;
        if (context.performed) OnFireInstantPerformed?.Invoke();
    }
    public void OnFireHold(InputAction.CallbackContext context)
    {
        if (!ShouldTriggerEvents) return;
        if (context.started) OnFireHoldStarted?.Invoke();
        if (context.canceled) OnFireHoldCanceled?.Invoke();
        if (context.performed) OnFireHoldPerformed?.Invoke();
    }
    public void OnFireCharge(InputAction.CallbackContext context)
    {
        if (!ShouldTriggerEvents) return;
        if (context.started) OnFireChargeStarted?.Invoke();
        if (context.canceled) OnFireChargeCanceled?.Invoke();
        if (context.performed) OnFireChargePerformed?.Invoke();
    }
}