using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[ExecuteAlways]
public class KeepInPlace : MonoBehaviour
{
    [SerializeField] private Transform followObject;
    [SerializeField] private Vector3 offset;

    void LateUpdate()
    {
        transform.position = followObject.position + offset;
        transform.rotation = Quaternion.identity;
    }
}
