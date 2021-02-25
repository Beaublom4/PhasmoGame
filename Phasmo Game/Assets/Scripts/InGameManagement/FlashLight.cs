using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FlashLight : Item
{
    public Light light;
    
    [PunRPC]
    public void TurnOn()
    {
        if (!PV.IsMine)
            return;
        turnedOn = false;
        light.enabled = true;
    }
    [PunRPC]
    public void TurnOff()
    {
        if (!PV.IsMine)
            return;
        turnedOn = true;
        light.enabled = false;
    }
}
