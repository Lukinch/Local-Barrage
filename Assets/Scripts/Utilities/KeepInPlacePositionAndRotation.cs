
using UnityEngine;

[ExecuteAlways]
public class KeepInPlacePositionAndRotation : MonoBehaviour
{
    [SerializeField] private Transform followObject;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;

    private void Update()
    {
        transform.SetPositionAndRotation(followObject.position + positionOffset, Quaternion.Euler(rotationOffset));
    }
}
