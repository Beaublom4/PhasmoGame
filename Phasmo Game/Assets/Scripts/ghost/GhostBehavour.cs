using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public class GhostBehavour : MonoBehaviour
{
    //public
    [Header("Ghost components")]
    public GhostType ghostType;
    [Range(0, 20)]
    public float sphereRadius;
    public LayerMask doorMask;
    public LayerMask electricObjectMask;
    public LayerMask playerMask;
    [Header("MapComponants")]
    public Transform room;
    [Header("Ghost VisableWalkPoints")]
    public List<Transform> walkingPoints;
    [Header("Lists")]
    public List<GameObject> players;
    public List<GameObject> lights;
    public List<GameObject> skinList;
    public List<Collider> doors=new List<Collider>();
    public List<Collider> electricObjects = new List<Collider>();
    public List<UnityEvent> ghostEvents;
    [Header("Window event")]
    public Transform windowPos;
    //private
    private int doorCount;

    private float distance;

    private bool hunting;
    private GameObject nearestPlayer;

    private NavMeshAgent agent;
    private Animator anim;

    void Start()
    {
        //if (!PhotonNetwork.IsMasterClient)
        //    return;
        agent = GetComponent<NavMeshAgent>();
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        lights.AddRange(GameObject.FindGameObjectsWithTag("Light"));
        anim = gameObject.GetComponent<Animator>();
        VisabilityOff();
        agent.destination = room.position;
    }
    private void Update()
    {
        if (hunting)
        {
            agent.destination = nearestPlayer.transform.position;
        }
    }
    public void Hunt()
    {
        GetClosesPlayer();
        hunting = true;
        Huntvisabilty();
        anim.SetBool("isDoingRunning", true);
    }
    public void Huntvisabilty()
    {
        if (hunting)
        {
            Invoke("VisabilityOff", 0.5f);
            Invoke("VisabilityOn", 1);
            Invoke("Huntvisabilty", 1.5f);
        }
    }
    public void StopHunting()
    {
        hunting = false;
    }
    public void GetClosesPlayer()
    {
        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (nearestPlayer == null)
            {
                nearestPlayer = player;
                distance = dist;
            }
            else
            {
                if (dist < distance)
                {
                    nearestPlayer = player;
                    distance = dist;
                }
            }
        }
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
    public void ElectricBlast()
    {
        Collider[] currentElectricObjects = Physics.OverlapSphere(transform.position, sphereRadius,electricObjectMask);

        foreach (Collider collider in currentElectricObjects)
        {
            electricObjects.Add(collider);
        }
        foreach (Collider door in electricObjects)
        {
            //doe hier afgaan   
        }        
    }
    public void TurnOffLightEvent()
    {
        ResetAnim();
        anim.SetBool("isDoingOffLights", true);
    }
    public void TurnOffLights()
    {
        foreach(GameObject light in lights)
        {
            light.SetActive(false);         
        }
    }
    public void VisabilityOff()
    {
        foreach (GameObject skin in skinList)
        {
            skin.SetActive(false);
        }
    }
    public void VisabilityOn()
    {
        foreach (GameObject skin in skinList)
        {
            skin.SetActive(true);
        }
    }
    public void RandomVisableInRoom()
    {
        GetClosesPlayer();
        if (distance <= 10)
        {
        ResetAnim();
        VisabilityOn();
        transform.position = nearestPlayer.transform.position;
        int random = Random.Range(1, 3);
        if (random == 1)
        {
            anim.SetBool("isDoingPain", true);
        }
        else if (random == 2)
        {
            anim.SetBool("isDoingScreaming", true);
        }
        Invoke("ResetValues", ghostType.randomEventTime);
        }
        else
        {
            RandomEvent();
        }
    }
    public void WindowEvent()
    {
        VisabilityOn();
        transform.position = windowPos.position;
        GetClosesPlayer();
        transform.LookAt(nearestPlayer.transform);
        Invoke("VisabilityOff", ghostType.windowtime);
        ResetAnim();
        anim.SetBool("isDoingScreaming", true);
    }
    public void Walk()
    {
        VisabilityOn();
        int randomNumber = Random.Range(0, walkingPoints.Count);
        agent.destination = walkingPoints[randomNumber].position;
        ResetAnim();
        anim.SetBool("isDoingWalking", true);
    }
    public void ResetValues()
    {
        doors.Clear();
        distance = 0;
        nearestPlayer = null;
        ResetAnim();
    }
    public void ResetAnim()
    {
        anim.SetBool("isDoingIdle", false);
        anim.SetBool("isDoingRunning", false);
        anim.SetBool("isDoingWalking", false);
        anim.SetBool("isDoingPain", false);
        anim.SetBool("isDoingScreaming", false);
        anim.SetBool("isDoingOffLights", false);
    }
    public void RandomEvent()
    {
        int randomInt = Random.Range(0, ghostEvents.Count);
        ghostEvents[randomInt]?.Invoke();
    }
    private void OnDrawGizmos()
    {
        
    }
}