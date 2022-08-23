using UnityEngine;
using TMPro;

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
    [Header("Menu UI Related")]
    [SerializeField] private UIPlayerMainMenu _playerUiManager;
    [Header("Gameplay Related")]
    [SerializeField] private Billboard _billboard;
    [SerializeField] private KeepInPlacePositionAndRotation _keepInPlacePositionAndRotation;
    [SerializeField] private PlayerMoveController _moveController;
    [SerializeField] private PlayerPoints _points;
    [SerializeField] private PlayerTurretController _turretController;
    [SerializeField] private TurretFiringController _turretFiringController;
    [SerializeField] private PlayerTurretRotationController _turretRotationController;

    #region Objects
    public GameObject MenuUI { get => _menuUI; }
    public GameObject LiveUI { get => _liveUI; }
    public GameObject TurretPosition { get => _turretPosition; }
    public GameObject PlayerColliders { get => _playerColliders; }
    #endregion

    #region Scripts
    #region Menu UI Related
    public UIPlayerMainMenu PlayerUiManager { get => _playerUiManager; }
    public TextMeshProUGUI MenuPlayerName;
    #endregion

    #region Gameplay Related
    public MeshRenderer SphereRenderer { get => _sphereRenderer; }
    public Billboard Billboard { get => _billboard; }
    public KeepInPlacePositionAndRotation KeepInPlacePositionAndRotation { get => _keepInPlacePositionAndRotation; }
    public PlayerMoveController MoveController { get => _moveController; }
    public PlayerPoints Points { get => _points; }
    public TurretFiringController TurretFiringController { get => _turretFiringController; }
    public PlayerTurretController TurretController { get => _turretController; }
    public PlayerTurretRotationController TurretRotationController { get => _turretRotationController; }
    public TextMeshProUGUI GameplayPlayerName;
    #endregion
    #endregion
}
