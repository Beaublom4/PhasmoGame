using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.IO;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text playerName, roomName;
    [SerializeField] GameObject playerItem, startButton;
    [SerializeField] Transform playerList;
    [SerializeField] Acount acount;

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
        acount.SetData();
    }
    public void ChangeNickName(TMP_InputField _inputField)
    {
        string name = _inputField.text;
        PhotonNetwork.NickName = name;
        print("Changed name to: " + name);
        acount.SetData();
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

        CheckIfNameAvailable(0);

        roomName.text = PhotonNetwork.CurrentRoom.Name;
        if (!PhotonNetwork.IsMasterClient)
            startButton.SetActive(false);

        //player info spawn here
        PV.RPC("InstantiatePlayerItem", RpcTarget.AllBuffered, acount.playerName, acount.playerLevel, acount.character);
        MenuSwitcher.Instance.SwitchPanel("room");
    }
    void CheckIfNameAvailable(int nameNum)
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (p == PhotonNetwork.NetworkingClient.LocalPlayer)
                continue;
            if (p.NickName == PhotonNetwork.NickName)
            {
                print("Probleem");
                PhotonNetwork.NickName = PhotonNetwork.NickName + nameNum.ToString();
                int newNameNum = nameNum;
                newNameNum++;
                CheckIfNameAvailable(newNameNum);
            }
        }
    }
    [PunRPC]
    void InstantiatePlayerItem(string pName, int pLv, int character)
    {
        GameObject g = Instantiate(playerItem, playerList);
        PlayerItem item = g.GetComponent<PlayerItem>();
        item.playerName.text = pName;
        item.playerLevel.text = pLv.ToString();
    }
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void LeaveRoom()
    {
        foreach(Transform child in playerList)
        {
            if (child.GetComponent<PlayerItem>().playerName.text == PhotonNetwork.NickName)
            {
                Destroy(child.gameObject);
                break;
            }
        }

        PhotonNetwork.LeaveRoom();
        MenuSwitcher.Instance.SwitchPanel("loaing");
    }
    public override void OnLeftRoom()
    {
        print("Left room");
        MenuSwitcher.Instance.SwitchPanel("main");
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}