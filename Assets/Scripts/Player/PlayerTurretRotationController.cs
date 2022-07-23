using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTurretRotationController : MonoBehaviour
{
    //[SerializeField] private Transform objectToRotate;
    [SerializeField] private LayerMask mouseWorldLayerMask;

    private bool isInputMouse;
    private Vector2 mousePosition;
    private Vector2 gamepadValue;
    private Vector3 worldPoint;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void SetNewCamera(Camera newCamera)
    {
        mainCamera = newCamera;
    }

    private void Update()
    {
        if (!isInputMouse) HandleGamepadInput();
        if (isInputMouse) HandleMouseInput();
    }

    public void OnTurretRotation(InputAction.CallbackContext context)
    {
        isInputMouse = context.control.device.name == "Mouse";

        if (isInputMouse) mousePosition = context.ReadValue<Vector2>();
        else 
        {
            Vector2 value = context.ReadValue<Vector2>();
            if (value != Vector2.zero)
                gamepadValue = context.ReadValue<Vector2>();
        }
    }

    private void HandleMouseInput()
    {
       worldPoint = GetMouseInWorld();
       Vector3 targetPosition = new Vector3(worldPoint.x, transform.position.y, worldPoint.z);
       transform.LookAt(targetPosition);
    }

    private Vector3 GetMouseInWorld()
    {
       Ray ray = mainCamera.ScreenPointToRay(mousePosition);
       Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, mouseWorldLayerMask);
       return raycastHit.point;
    }

    private void HandleGamepadInput()
    {
        float angleRadians = Mathf.Atan2(gamepadValue.y, gamepadValue.x);
        float angleDegrees = -angleRadians * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angleDegrees + 90, Vector3.up);
    }
}
