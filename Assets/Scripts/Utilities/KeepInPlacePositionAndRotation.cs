
using UnityEngine;

[ExecuteAlways]
public class KeepInPlacePositionAndRotation : MonoBehaviour
{
    [SerializeField] private UpdateType _updateType;
    [SerializeField] private Transform _followObject;
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private Vector3 _rotationOffset;

    private void Update()
    {
        if (_updateType == UpdateType.Update)
        {
            transform.SetPositionAndRotation(_followObject.position + _positionOffset, Quaternion.Euler(_rotationOffset));
        }
    }
    private void FixedUpdate()
    {
        if (_updateType == UpdateType.FixedUpdate)
        {
            transform.SetPositionAndRotation(_followObject.position + _positionOffset, Quaternion.Euler(_rotationOffset));
        }
    }
    private void LateUpdate()
    {
        if (_updateType == UpdateType.LateUpdate)
        {
            transform.SetPositionAndRotation(_followObject.position + _positionOffset, Quaternion.Euler(_rotationOffset));
        }
    }

    enum UpdateType
    {
        Update,
        FixedUpdate,
        LateUpdate
    }
}
