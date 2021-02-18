using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerInteract : MonoBehaviour
{
    public GameObject itemInHand, emfReader;
    [SerializeField] Transform itemLoc;

    [SerializeField] GameObject cam;
    [SerializeField] float interactRange;
    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange, -5, QueryTriggerInteraction.Ignore))
        {
            if(hit.collider.tag == "Item")
            {
                if (Input.GetButtonDown("PickUp"))
                {
                    Instantiate(emfReader, itemLoc);
                    Destroy(hit.collider.gameObject);
                    //PhotonNetwork.Instantiate(Path.Combine("Items", hit.collider.GetComponent<Item>().itemName), itemLoc.position, itemLoc.rotation);
                    //PhotonNetwork.Destroy(hit.collider.gameObject);
                }
            }
            if(hit.collider.tag == "Breaker")
            {
                if (Input.GetButtonDown("PickUp"))
                {
                    GetComponent<Breaker>().Use();
                }
            }
        }
    }
}
