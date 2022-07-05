using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlayerTurretRotation : MonoBehaviour
{
    [SerializeField] private InputReaderSO inputReader;
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private LayerMask mouseWorldLayerMask;

    private bool isInputMouse;
    private Vector2 mousePosition;
    private Vector2 gamepadValue;
    private Vector3 worldPoint;
    private Camera mainCamera;
    
    private void OnEnable()
    {
        inputReader.TurretRotationMouseEvent += OnMouseRotationChanged;
        //input.TurretRotationGamepadEvent += OnGamepadRotationChanged;
    }
    private void OnDisable()
    {
        inputReader.TurretRotationMouseEvent -= OnMouseRotationChanged;
        //input.TurretRotationGamepadEvent -= OnGamepadRotationChanged;
    }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        worldPoint = GetMouseInWorld();
        Vector3 targetPosition = new Vector3(worldPoint.x, objectToRotate.position.y, worldPoint.z);
        objectToRotate.LookAt(targetPosition);
    }

    private Vector3 GetMouseInWorld()
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, mouseWorldLayerMask);
        return raycastHit.point;
    }

    private void OnMouseRotationChanged(Vector2 value) => mousePosition = value;
    private void OnGamepadRotationChanged(Vector2 value) => gamepadValue = value;
}
