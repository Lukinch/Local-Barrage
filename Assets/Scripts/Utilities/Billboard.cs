
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    public void SetNewCamera(Camera newCamera)
    {
        _cameraTransform = newCamera.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _cameraTransform.forward);
    }
}
