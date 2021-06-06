using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Billboard : MonoBehaviour
{
    Transform mainCameraTransform;

    private void LateUpdate()
    {
        mainCameraTransform = Camera.main.transform;
        transform.LookAt(this.transform.position + mainCameraTransform.rotation * Vector3.forward, mainCameraTransform.rotation * Vector3.up);
    }
}
