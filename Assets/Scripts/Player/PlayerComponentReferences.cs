using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponentReferences : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject liveUI;
    [SerializeField] private GameObject visuals;
    [SerializeField] private GameObject playerColliders;

    [Header("Scripts")]
    [SerializeField] private Billboard billboard;
    [SerializeField] private KeepInPlacePositionAndRotation keepInPlacePositionAndRotation;
    [SerializeField] private PlayerMoveController moveController;
    [SerializeField] private PlayerPoints points;
    [SerializeField] private PlayerTurretController turretController;
    [SerializeField] private PlayerTurretRotationController turretRotationController;

    public GameObject MenuUI { get => menuUI; }
    public GameObject LiveUI { get => liveUI; }
    public GameObject Visuals { get => visuals; }
    public GameObject PlayerColliders { get => playerColliders; }

    public Billboard Billboard { get => billboard; }
    public KeepInPlacePositionAndRotation KeepInPlacePositionAndRotation { get => keepInPlacePositionAndRotation; }
    public PlayerMoveController MoveController { get => moveController; }
    public PlayerPoints Points { get => points; }
    public PlayerTurretController TurretController { get => turretController; }
    public PlayerTurretRotationController TurretRotationController { get => turretRotationController; }
}
