using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowRotater : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        gameObject.transform.Rotate(new Vector3(0, 0, mouseX));

    }
}
