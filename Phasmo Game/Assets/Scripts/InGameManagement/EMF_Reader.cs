using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMF_Reader : Item
{
    public Transform rayPos;
    public float range;
    public LayerMask hittableMask;
    public void Update()
    {
        if(turnedOn == true)
        {
            RaycastHit hit;
            if(Physics.Raycast(rayPos.position, rayPos.forward, out hit, range, hittableMask))
            {
                if(hit.collider.tag == "EMF_Hitbox")
                {
                    print("piep piep");
                }
            }
        }
    }
}
