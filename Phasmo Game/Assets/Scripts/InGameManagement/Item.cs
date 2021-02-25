using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Item : MonoBehaviour
{
    [HideInInspector] public PhotonView PV;
    public string itemName;
    public bool turnedOn, pickedUp;

    public Color onColor, offColor;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public void TurnOn_Off()
    {
        if (turnedOn == true)
        {
            PV.RPC("TurnOn", RpcTarget.AllBuffered);
        }
        else
        {
            PV.RPC("TurnOff", RpcTarget.AllBuffered);
        }
    }
}
