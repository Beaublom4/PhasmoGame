using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool doorIsOpen;
    private void Start()
    {
        DoorAction();
    }
    public void DoorAction()
    {
        doorIsOpen = !doorIsOpen;
    }
}