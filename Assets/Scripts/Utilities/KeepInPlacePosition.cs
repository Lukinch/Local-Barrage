using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[ExecuteAlways]
public class KeepInPlacePosition : MonoBehaviour
{
    [SerializeField] private Transform followObject;
    [SerializeField] private Vector3 offset;

    private void FixedUpdate()
    {
        transform.position = followObject.position + offset;
        transform.rotation = Quaternion.Euler(new Vector3(0, followObject.rotation.y, 0));
    }
}
