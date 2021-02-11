using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInfo : MonoBehaviour
{
    public int playerLv;
    public 

	PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (PV.IsMine)
        {
            PV.RPC("GetInfo", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void GetInfo()
    {
        print("Get info");
    }
}