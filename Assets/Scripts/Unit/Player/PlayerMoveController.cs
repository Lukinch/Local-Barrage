using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    [SerializeField] private float forceStrenght;

    [SerializeField] private Rigidbody unitRB;

    private Vector2 movement;

    private void FixedUpdate()
    {
        MoveUnit();
    }

    private void MoveUnit()
    {
        unitRB.AddForce(new Vector3(movement.x, 0, movement.y) * forceStrenght);
    }

    public void StopMovement()
    {
        unitRB.velocity = Vector3.zero;
        unitRB.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// Called by Player Input component with Unity Events to
    /// process controller inputs
    /// </summary>
    /// <param name="context">All the data from the input action</param>
    public void OnMovement(InputAction.CallbackContext context) 
    {
        movement = context.ReadValue<Vector2>();
    }
}
