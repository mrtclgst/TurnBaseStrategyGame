using System;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private bool _invert = true;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (_invert)
        {
            Vector3 dirToCam = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCam * -1);
        }
        else
        {
            //transform.LookAt(cameraTransform);
            transform.forward = Camera.main.transform.forward;
        }
    }
}