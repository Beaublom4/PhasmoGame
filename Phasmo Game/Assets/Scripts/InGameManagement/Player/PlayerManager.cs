using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    RoomManager RM;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        RM = FindObjectOfType<RoomManager>();
    }
    private void Start()
    {
        if (!PV.IsMine)
            return;

        PhotonNetwork.Instantiate(Path.Combine("Player", "PlayerController"), RM.spawnPoint.position, RM.spawnPoint.rotation);
    }
}
