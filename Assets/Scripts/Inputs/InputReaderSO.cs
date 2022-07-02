using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Scriptable Objects/InputReaderSO", fileName = "InputReaderSO", order = 0)]
public class InputReaderSO :
    ScriptableObject,
    GameInputs.IVehicleActions
{
    #region Vehicle Events
    public event Action<Vector2> MovementEvent;
    public event Action<bool> BrakeEvent;
    #endregion

    #region Turret Events
    public event Action FireInstantEvent;
    public event Action FireChargeEventStarted;
    public event Action FireChargeEventCanceled;
    public event Action FireChargeEventPerformed;
    public event Action FireHoldEventStarted;
    public event Action FireHoldEventCanceled;
    public event Action FireHoldEventPerformed;
    public event Action<Vector2> TurretRotationMouseEvent; 
    public event Action<Vector2> TurretRotationGamepadEvent; 
    #endregion
    
    private GameInputs _gameInputs;
    
    private void OnEnable()
    {
        if (_gameInputs == null)
        {
            _gameInputs = new GameInputs();
            _gameInputs.Vehicle.SetCallbacks(this);
        }

        EnableAllInputs();
    }
    private void OnDisable() => DisableAllInputs();
    
    #region Vehicle Inputs reading and Events invocation
    public void OnBrake(InputAction.CallbackContext context)
    {
        BrakeEvent?.Invoke(context.ReadValue<float>() != 0);
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementEvent?.Invoke(context.ReadValue<Vector2>());
    }
    #endregion
    
    #region Turret Inputs reading and Events invocation
    public void OnTurretRotation(InputAction.CallbackContext context)
    {
        // TODO: Rework rotation events behaviour, right now it doesn't detect
        // switching inputs when the mouse is active.
        bool isDeviceMouse = context.control.device.name == "Mouse";
        if (isDeviceMouse) TurretRotationMouseEvent?.Invoke(context.ReadValue<Vector2>());
        else TurretRotationGamepadEvent?.Invoke(context.ReadValue<Vector2>());
    }
    public void OnTurretFireInstant(InputAction.CallbackContext context)
    {
        if (context.performed) FireInstantEvent?.Invoke();
    }
    public void OnTurretFireCharge(InputAction.CallbackContext context)
    {
        if (context.started) FireChargeEventStarted?.Invoke();
        if (context.canceled) FireChargeEventCanceled?.Invoke();
        if (context.performed) FireChargeEventPerformed?.Invoke();
    }
    public void OnTurretFireHold(InputAction.CallbackContext context)
    {
        if (context.started) FireHoldEventStarted?.Invoke();
        if (context.canceled) FireHoldEventCanceled?.Invoke();
        if (context.performed) FireHoldEventPerformed?.Invoke();
    }
    #endregion

    #region Input maps enable and disable
    private void EnableVehicleInput() => _gameInputs.Vehicle.Enable();
    private void DisableVehicleInput() =>  _gameInputs.Vehicle.Disable();

    private void EnableAllInputs()
    {
        EnableVehicleInput();
    }
    private void DisableAllInputs()
    {
        DisableVehicleInput();
    }
    #endregion
}
