
using UnityEngine;

[ExecuteAlways]
public class KeepInPlacePosition : MonoBehaviour
{
    [SerializeField] private Transform _followObject;
    [SerializeField] private Vector3 _offset;

    private void FixedUpdate()
    {
        transform.position = _followObject.position + _offset;
    }
}
