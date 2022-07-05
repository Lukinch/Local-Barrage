using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlayerMover : MonoBehaviour
{
    [SerializeField] private InputReaderSO inputReader;
    [SerializeField] private float forceStrenght;

    [SerializeField] private Rigidbody unitRB;

    private Vector2 movement;
    
    private void OnEnable() => inputReader.MovementEvent += OnMovementEvent;
    private void OnDisable() => inputReader.MovementEvent -= OnMovementEvent;
    private void OnMovementEvent(Vector2 value) => movement = value;

    private void FixedUpdate()
    {
        MoveUnit();
    }

    private void MoveUnit()
    {
        unitRB.AddForce(new Vector3(movement.x, 0, movement.y) * forceStrenght);
    }

}
