using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float closedRot, openedRot;
    public float roty;

    public void Rotate(Vector3 pos)
    {
        transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));
        roty = transform.eulerAngles.y;
        roty = Mathf.Clamp(roty, closedRot, openedRot);
        transform.rotation = Quaternion.Euler(Quaternion.identity.x, roty, Quaternion.identity.z);
    }
}
