using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Acount : MonoBehaviour
{
    public string playerName;
    public int playerLevel;
    public int character;

    public void SetData()
    {
        playerName = PhotonNetwork.NickName;
    }
}
