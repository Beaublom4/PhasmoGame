using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;
using TMPro;
using System.IO;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text playerName, roomName;
    [SerializeField] GameObject playerItem, startButton;
    [SerializeField] Transform playerList, voicesList;
    [SerializeField] Acount acount;

    public List<int> lvls = new List<int>();
    public List<int> characters = new List<int>();
    public bool buffered;
    IEnumerator bufferCoroutine;

    public PhotonVoiceView PVV;
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
    private void Update()
    {
        if(PhotonNetwork.InRoom)
        if(!buffered && lvls.Count == PhotonNetwork.PlayerList.Length)
        {
            buffered = true;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                GameObject g = Instantiate(playerItem, playerList);
                PlayerItem PI = g.GetComponent<PlayerItem>();
                PI.playerName.text = PhotonNetwork.PlayerList[i].NickName;
                PI.playerLevel.text = lvls[i].ToString();
                PI.UpdateSprite(characters[i]);
            }
        }
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

        PV.RPC("RPC_SendItemInfo", RpcTarget.AllBuffered, acount.playerLevel, acount.character);
        foreach(Transform child in playerList)
        {
            Destroy(child.gameObject);
        }
        //for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        //{
        //    GameObject g = Instantiate(playerItem, playerList);
        //    PlayerItem PI = g.GetComponent<PlayerItem>();
        //    PI.playerName.text = PhotonNetwork.PlayerList[i].NickName;
        //    //PI.playerLevel.text = lvls[i].ToString();
        //}

        MenuSwitcher.Instance.SwitchPanel("room");
    }
    [PunRPC]
    void RPC_SendItemInfo(int PL, int C)
    {
        lvls.Add(PL);
        characters.Add(C);
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
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        foreach (Transform child in playerList)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject g = Instantiate(playerItem, playerList);
            PlayerItem PI = g.GetComponent<PlayerItem>();
            PI.playerName.text = PhotonNetwork.PlayerList[i].NickName;
            StartCoroutine(UpdatePlayerItem(PI, i));
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        StartCoroutine(UpdateOnLeft());
    }
    IEnumerator UpdateOnLeft()
    {
        yield return new WaitForSeconds(.5f);
        foreach (Transform child in playerList)
        {
            Destroy(child.gameObject);
        }
        print(PhotonNetwork.PlayerList.Length);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject g = Instantiate(playerItem, playerList);
            PlayerItem PI = g.GetComponent<PlayerItem>();
            PI.playerName.text = PhotonNetwork.PlayerList[i].NickName;
            StartCoroutine(UpdatePlayerItem(PI, i));
        }
    }
    IEnumerator UpdatePlayerItem(PlayerItem PI, int i)
    {
        yield return new WaitForSeconds(.5f);
        PI.playerLevel.text = lvls[i].ToString();
        PI.UpdateSprite(characters[i]);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void LeaveRoom()
    {
        //foreach(Transform child in playerList)
        //{
        //    if (child.GetComponent<PhotonView>().IsMine)
        //    {
        //        PhotonNetwork.Destroy(child.gameObject);
        //        continue;
        //    }
        //    Destroy(child.gameObject);
        //    break;
        //}
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                PV.RPC("RPC_RemoveItemStat", RpcTarget.OthersBuffered, i);
            }
        }
        lvls.Clear();
        characters.Clear();
        buffered = false;
        //PhotonNetwork.Destroy(PVV.gameObject);
        PhotonNetwork.LeaveRoom();
        MenuSwitcher.Instance.SwitchPanel("loading");
    }
    [PunRPC]
    void RPC_RemoveItemStat(int i)
    {
        lvls.RemoveAt(i);
        characters.RemoveAt(i);
    }
    public override void OnLeftRoom()
    {
        print("Left room");
        MenuSwitcher.Instance.SwitchPanel("main");
    }
    private void OnApplicationQuit()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                PV.RPC("RPC_RemoveItemStat", RpcTarget.OthersBuffered, i);
            }
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Customize()
    {
        MenuSwitcher.Instance.SwitchPanel("characters");
    }
    public void Back()
    {
        MenuSwitcher.Instance.SwitchPanel("main");
    }
}