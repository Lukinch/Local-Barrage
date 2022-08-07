
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveController : MonoBehaviour
{
    [SerializeField] private float _forceStrenght;

    [SerializeField] private Rigidbody _unitRB;

    private Vector2 _movement;

    private void FixedUpdate()
    {
        MoveUnit();
    }

    private void MoveUnit()
    {
        _unitRB.AddForce(new Vector3(_movement.x, 0, _movement.y) * _forceStrenght);
    }

    public void StopMovement()
    {
        _unitRB.velocity = Vector3.zero;
        _unitRB.angularVelocity = Vector3.zero;
    }

    public void EnableRigidBody() => _unitRB.isKinematic = false;
    public void DisableRigidBody() => _unitRB.isKinematic = true;

    /// <summary>Called by Player Input Events</summary>
    public void OnMovement(InputAction.CallbackContext context) 
    {
        _movement = context.ReadValue<Vector2>();
    }
}
