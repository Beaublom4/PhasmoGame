using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerInteract : MonoBehaviour
{
    public GameObject itemInHand;
    [SerializeField] Transform itemLoc, dropLoc;
    [SerializeField] float dropForce;

    [SerializeField] GameObject cam;
    [SerializeField] float interactRange;

    RaycastHit hit;

    public Transform cube;
    bool interactDoor;
    Door doorScript;
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
                GameObject g = PhotonNetwork.Instantiate(Path.Combine("Items", itemInHand.GetComponent<Item>().itemName), dropLoc.position, dropLoc.rotation);
                itemInHand.GetComponent<Item>().pickedUp = false;
                Rigidbody g_rb = g.GetComponent<Rigidbody>();
                g_rb.AddRelativeForce(0, 0, dropForce);
                if (itemInHand.GetComponent<Item>().turnedOn)
                    g.GetComponent<Item>().TurnOn_Off();
                Destroy(itemInHand);
            }
        }
        Debug.DrawRay(cam.transform.position, cam.transform.forward * interactRange, Color.green);
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactRange, -5, QueryTriggerInteraction.Ignore))
        {
            if (!GetComponent<PhotonView>().IsMine)
                return;
            if(hit.collider.tag == "Item")
            {
                if (Input.GetButtonDown("PickUp"))
                {
                    if (itemInHand != null)
                        return;
                    PickUpItem();
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
            if(hit.collider.tag == "Door")
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    interactDoor = true;
                    doorScript = hit.collider.GetComponentInParent<Door>();
                }
            }
        }

        if(interactDoor == true)
        {
            if (Input.GetButtonUp("Fire1"))
            {
                interactDoor = false;
            }

            print("Interacting");
            doorScript.Rotate(cube.position);
        }
    }
    [PunRPC]
    void PickUpItem()
    {
        itemInHand = PhotonNetwork.Instantiate(Path.Combine("Items", hit.collider.GetComponent<Item>().itemName), itemLoc.position, itemLoc.rotation);
        itemInHand.GetComponent<Collider>().enabled = false;
        itemInHand.GetComponent<Item>().pickedUp = true;
        itemInHand.transform.SetParent(itemLoc);
        Rigidbody rb = itemInHand.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        if (hit.collider.GetComponent<Item>().turnedOn)
            itemInHand.GetComponent<Item>().TurnOn_Off();
        PhotonNetwork.Destroy(hit.collider.gameObject);
    }
}
