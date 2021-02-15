using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class MapCreator : MonoBehaviour
{
    [Header("Variables")]

    public GameObject listHolder;

    private GameObject ghost;
    private GameObject room;
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        ChooseGhost();
        ChooseRoom();
    }
    public void ChooseGhost()
    {
        var randomInt = Random.Range(0, listHolder.GetComponent<GhostList>().ghostName.Count);

        ghost = listHolder.GetComponent<GhostList>().ghost[randomInt];
    }
    public void ChooseRoom()
    {
        var randomInt = Random.Range(0, listHolder.GetComponent<RoomList>().rooms.Count);

        room = listHolder.GetComponent<RoomList>().rooms[randomInt];

        PhotonNetwork.Instantiate(Path.Combine("Ghost", ghost.GetComponent<GhostBehavour>().ghostType.ghostName), room.transform.position, Quaternion.identity);
    }
}