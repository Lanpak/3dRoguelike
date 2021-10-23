using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform cameraHolderTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform arrowTransform;

    private float xRot;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);

        
        cameraHolderTransform.localRotation = Quaternion.Euler(new Vector3(xRot, 0, 0));


        

        playerTransform.Rotate(new Vector3(0, mouseX, 0));

    }
}
