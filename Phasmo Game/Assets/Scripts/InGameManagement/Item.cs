using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public bool turnedOn;

    public Color onColor, offColor;

    public void TurnOn_Off()
    {
        if(turnedOn == true)
        {
            turnedOn = false;
            GetComponent<MeshRenderer>().material.color = offColor;
        }
        else
        {
            turnedOn = true;
            GetComponent<MeshRenderer>().material.color = onColor;
        }
    }
}
