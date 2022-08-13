
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTurretRotationController : MonoBehaviour
{
    [SerializeField] private LayerMask _mouseWorldLayerMask;

    private bool _isInputMouse;
    private Vector2 _mousePosition;
    private Vector2 _gamepadValue;
    private Vector3 _worldPoint;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void SetNewCamera(Camera newCamera)
    {
        _mainCamera = newCamera;
    }

    private void Update()
    {
        if (_mainCamera && !_isInputMouse) HandleGamepadInput();
        if (_mainCamera && _isInputMouse) HandleMouseInput();
    }

    public void OnTurretRotation(InputAction.CallbackContext context)
    {
        _isInputMouse = context.control.device.name == "Mouse";

        if (_isInputMouse) _mousePosition = context.ReadValue<Vector2>();
        else
        {
            Vector2 value = context.ReadValue<Vector2>();
            if (value != Vector2.zero)
                _gamepadValue = context.ReadValue<Vector2>();
        }
    }

    private void HandleMouseInput()
    {
        _worldPoint = GetMouseInWorld();
        Vector3 targetPosition = new Vector3(_worldPoint.x, transform.position.y, _worldPoint.z);
        transform.LookAt(targetPosition);
    }

    private Vector3 GetMouseInWorld()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _mouseWorldLayerMask);
        return raycastHit.point;
    }

    private void HandleGamepadInput()
    {
        float angleRadians = Mathf.Atan2(_gamepadValue.y, _gamepadValue.x);
        float angleDegrees = -angleRadians * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angleDegrees + 90, Vector3.up);
    }
}
