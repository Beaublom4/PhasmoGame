using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (!PV.IsMine)
            return;

        PhotonNetwork.Instantiate(Path.Combine("Player", "PlayerController"), Vector3.zero, Quaternion.identity);
    }
}
