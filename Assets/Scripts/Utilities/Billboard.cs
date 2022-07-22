
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    public void SetNewCamera(Camera newCamera)
    {
        cameraTransform = newCamera.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
