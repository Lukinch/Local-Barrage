using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponentReferences : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _liveUI;
    [SerializeField] private GameObject _playerColliders;
    [SerializeField] private GameObject _turretPosition;

    [Header("Mesh Renderers")]
    [SerializeField] private MeshRenderer _sphereRenderer;

    [Header("Scripts")]
    [SerializeField] private Billboard _billboard;
    [SerializeField] private KeepInPlacePositionAndRotation _keepInPlacePositionAndRotation;
    [SerializeField] private PlayerMoveController _moveController;
    [SerializeField] private PlayerPoints _points;
    [SerializeField] private PlayerTurretController _turretController;
    [SerializeField] private TurretFiringController _turretFiringController;
    [SerializeField] private PlayerTurretRotationController _turretRotationController;

    public GameObject MenuUI { get => _menuUI; }
    public GameObject LiveUI { get => _liveUI; }
    public GameObject TurretPosition { get => _turretPosition; }
    public MeshRenderer SphereRenderer { get => _sphereRenderer; }
    public GameObject PlayerColliders { get => _playerColliders; }
    public Billboard Billboard { get => _billboard; }
    public KeepInPlacePositionAndRotation KeepInPlacePositionAndRotation { get => _keepInPlacePositionAndRotation; }
    public PlayerMoveController MoveController { get => _moveController; }
    public PlayerPoints Points { get => _points; }
    public TurretFiringController TurretFiringController { get => _turretFiringController; }
    public PlayerTurretController TurretController { get => _turretController; }
    public PlayerTurretRotationController TurretRotationController { get => _turretRotationController; }
}
