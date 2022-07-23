using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[ExecuteAlways]
public class KeepInPlacePositionAndRotation : MonoBehaviour
{
    [SerializeField] private Transform followObject;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotationOffset;

    private void FixedUpdate()
    {
        transform.position = followObject.position + positionOffset;
        transform.eulerAngles = rotationOffset;
    }
}
