using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool doorIsOpen;
    
    public void OpenDoor()
    {
        doorIsOpen = true;
    }
    public void CloseDoor()
    {
        doorIsOpen = false;
    }
}