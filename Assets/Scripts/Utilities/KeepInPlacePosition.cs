
using UnityEngine;

[ExecuteAlways]
public class KeepInPlacePosition : MonoBehaviour
{
    [SerializeField] private Transform followObject;
    [SerializeField] private Vector3 offset;

    private void Update()
    {
        transform.position = followObject.position + offset;
    }
}
