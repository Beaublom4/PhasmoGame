using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostBehavour : MonoBehaviour
{
    //public
    [Header("Ghost components")]
    public GhostType ghostType;
    [Header("Ghost VisableWalkPoints")]
    public List<Transform> walkingPoints;
    [Header("Ghost values")]
    [Header("Lists")]
    public List<GameObject> players;
    public List<GameObject> lights;

    [Header("Door values")]
    [Range(0, 20)]
    public float sphereRadius;
    public LayerMask doorMask;

    public List<Collider> doors=new List<Collider>();
    //private
    private int doorCount;

    private float distance;

    private GameObject nearestPlayer;

    private NavMeshAgent agent;

    void Start()
    {
        //if (!PhotonNetwork.IsMasterClient)
        //    return;

        agent = GetComponent<NavMeshAgent>();
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        lights.AddRange(GameObject.FindGameObjectsWithTag("Light"));
        Door();
    }
    private void Update()
    {
    }
    public void Hunt()
    {
        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (nearestPlayer = null)
            {
                nearestPlayer = player;
                distance = dist;
            }
            else
            {
                if (dist > distance)
                {
                    nearestPlayer = player;
                    distance = dist;
                }
            }
        }
        //pak nereast player als target
    }
    public void Door()
    {
        Collider[]currentDoors = Physics.OverlapSphere(transform.position, sphereRadius, doorMask);

        foreach(Collider collider in currentDoors)
        {
            doors.Add(collider);
        }
        foreach (Collider door in doors)
        {
            if (door.gameObject.GetComponent<DoorScript>().doorIsOpen)
            { 
                door.gameObject.GetComponent<DoorScript>().CloseDoor();
            }
            else
            {
                door.gameObject.GetComponent<DoorScript>().OpenDoor();
            }
        }
        ResetValues();
    }
    public void TurnOffLights()
    {
        foreach(GameObject light in lights)
        {
            light.SetActive(false);
        }
    }
    public void Walk()
    {
        agent.destination = walkingPoints[0].position;
    }
    public void ResetValues()
    {
        doors.Clear();
        distance = 0;
        nearestPlayer = null;
    }
    private void OnDrawGizmos()
    {
        
    }
}
