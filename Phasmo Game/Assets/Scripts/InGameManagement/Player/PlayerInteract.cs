using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerInteract : MonoBehaviour
{
    public GameObject itemInHand, item;
    [SerializeField] Transform itemLoc, dropLoc;
    [SerializeField] float dropForce;

    [SerializeField] GameObject cam;
    [SerializeField] float interactRange;
    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if(itemInHand != null)
            {
                itemInHand.GetComponent<Item>().TurnOn_Off();
            }
        }
        if (Input.GetButtonDown("Drop"))
        {
            if(itemInHand != null)
            {
                GameObject g = Instantiate(item, dropLoc.position, dropLoc.rotation, null);
                Rigidbody g_rb = g.GetComponent<Rigidbody>();
                g_rb.AddRelativeForce(0, 0, dropForce);
                if (itemInHand.GetComponent<Item>().turnedOn)
                    g.GetComponent<Item>().TurnOn_Off();
                Destroy(itemInHand);
            }
        }
        Debug.DrawRay(cam.transform.position, cam.transform.forward * interactRange, Color.green);
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange, -5, QueryTriggerInteraction.Ignore))
        {
            if(hit.collider.tag == "Item")
            {
                if (Input.GetButtonDown("PickUp"))
                {
                    itemInHand = Instantiate(item, itemLoc);
                    Rigidbody rb = itemInHand.GetComponent<Rigidbody>();
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    if (hit.collider.GetComponent<Item>().turnedOn)
                        itemInHand.GetComponent<Item>().TurnOn_Off();
                    Destroy(hit.collider.gameObject);
                    //PhotonNetwork.Instantiate(Path.Combine("Items", hit.collider.GetComponent<Item>().itemName), itemLoc.position, itemLoc.rotation);
                    //PhotonNetwork.Destroy(hit.collider.gameObject);
                }
                if (Input.GetButtonDown("Fire2"))
                {
                    hit.collider.GetComponent<Item>().TurnOn_Off();
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
