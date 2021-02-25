using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;
    public Transform spawnPoint;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    private void Start()
    {
        PhotonNetwork.Instantiate(Path.Combine("Player", "PlayerManager"), Vector3.zero, Quaternion.identity);
    }
}
