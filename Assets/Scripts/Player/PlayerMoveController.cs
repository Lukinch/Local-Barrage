
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
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

    /// <summary>Called by Player Input Events</summary>
    public void OnMovement(InputAction.CallbackContext context) 
    {
        movement = context.ReadValue<Vector2>();
    }
}
