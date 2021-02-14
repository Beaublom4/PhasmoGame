using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerView : MonoBehaviour
{
    public float sensitivity;

    Transform playerBody;
    float xRotation;

    PhotonView PV;
    private void Awake()
    {
        PV = GetComponentInParent<PhotonView>();
        playerBody = transform.parent;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= MouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
