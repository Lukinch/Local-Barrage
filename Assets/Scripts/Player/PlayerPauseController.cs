using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPauseController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    public static event Action<PlayerInput> OnPauseGame;

    /// <summary>Called by Player Input Events</summary>
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed) OnPauseGame?.Invoke(playerInput);
    }
}
