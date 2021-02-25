using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviour
{
    public float walkingSpeed, sprintMultiplier;

    float moveZ, moveX;

    Rigidbody rb;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            GetComponent<PlayerInteract>().enabled = false;
            GetComponent<PlayerMove>().enabled = false;
        }
        else
        {
            rb = GetComponent<Rigidbody>();
        }
    }
    private void LateUpdate()
    {
        if (!PV.IsMine)
            return;
        Move();
    }
    void Move()
    {
        moveZ = Input.GetAxis("Vertical");
        moveX = Input.GetAxis("Horizontal");

        float newSpeed;
        if (moveZ != 0 && moveX != 0)
            newSpeed = walkingSpeed * 0.75f;
        else
            newSpeed = walkingSpeed;
        if (Input.GetButton("Sprint"))
            newSpeed *= sprintMultiplier;

        Vector3 move = new Vector3(moveX, 0, moveZ) * newSpeed * Time.deltaTime;

        transform.Translate(move);
    }
}
