using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.IO;
using Photon.Realtime;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text playerName, roomName;
    [SerializeField] GameObject playerItem;
    [SerializeField] Transform playerList;
    [SerializeField] Transform playerInfos;

    PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        print("Connecting");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        print("Connected to master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        MenuSwitcher.Instance.SwitchPanel("main");
        print("Joined Lobby");
        if (PhotonNetwork.NickName == "")
        {
            string name = "Player" + Random.Range(0, 9999).ToString("0000");
            PhotonNetwork.NickName = name;
            playerName.text = name;
        }
        else
        {
            playerName.text = PhotonNetwork.NickName;
        }
    }
    public void ChangeNickName(TMP_InputField _inputField)
    {
        string name = _inputField.text;
        PhotonNetwork.NickName = name;
        print("Changed name to: " + name);
    }
    public void CreateRoom()
    {
        string name = "Room" + Random.Range(0, 999).ToString("000");
        PhotonNetwork.CreateRoom(name);
        MenuSwitcher.Instance.SwitchPanel("loading");
    }
    public override void OnJoinedRoom()
    {
        print("Joined Room");
        roomName.text = PhotonNetwork.CurrentRoom.Name;

        PV.RPC("CreatePlayerInfo", RpcTarget.AllBuffered);

        MenuSwitcher.Instance.SwitchPanel("room");

        Player[] players = PhotonNetwork.PlayerList;
        foreach(Transform child in playerList)
        {
            Destroy(child);
        }
        for (int i = 0; i < players.Length; i++)
        {
            //Instantiate(playerItem, playerList));
        }
    }
    [PunRPC]
    void CreatePlayerInfo()
    {
        GameObject g = PhotonNetwork.Instantiate("PlayerInfo", new Vector3(0, 0, 0), Quaternion.identity, 0);
        g.transform.SetParent(playerInfos);
    }
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
}