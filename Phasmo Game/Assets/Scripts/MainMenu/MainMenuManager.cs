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
    [SerializeField] GameObject playerItem, roomItem, startButton;
    [SerializeField] Transform playerList, roomsList;
    [SerializeField] Acount acount;
    [SerializeField] Customization customizeScript;

    public List<int> lvls = new List<int>();
    public List<int> characters = new List<int>();
    bool buffered;
    IEnumerator bufferCoroutine;

    [SerializeField] GameObject[] CharacterPrefabs;
    [SerializeField] Transform[] spawnPoints;

    public PhotonVoiceView PVV;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        print("Connecting");
        MenuSwitcher.Instance.SwitchPanel("loading");
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Update()
    {
        if (PhotonNetwork.InRoom)
            if (!buffered && lvls.Count == PhotonNetwork.PlayerList.Length)
            {
                buffered = true;
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    GameObject g = Instantiate(playerItem, playerList);
                    PlayerItem PI = g.GetComponent<PlayerItem>();
                    PI.playerName.text = PhotonNetwork.PlayerList[i].NickName;
                    if (PhotonNetwork.PlayerList[i].IsMasterClient)
                    {
                        PI.host.SetActive(true);
                    }
                    PI.playerLevel.text = lvls[i].ToString();
                    PI.UpdateSprite(characters[i]);
                    SpawnCharacterPreviews();
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
        customizeScript.SpawnAccountCharacter();
        MenuSwitcher.Instance.SwitchPanel("main");
    }
    public void ChangeNickName(TMP_InputField _inputField)
    {
        string name = _inputField.text;
        PhotonNetwork.NickName = name;
        print("Changed name to: " + name);
        acount.SetData();
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform t in roomsList)
        {
            Destroy(t.gameObject);
        }
        foreach(RoomInfo r in roomList)
        {
            if (r.PlayerCount > 0)
            {
                GameObject g = Instantiate(roomItem, roomsList);
                g.GetComponent<RoomItem>().SetUp(r.Name, r.PlayerCount);
            }
        }
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

        customizeScript.RemoveCharacter();

        CheckIfNameAvailable(0);

        roomName.text = PhotonNetwork.CurrentRoom.Name;
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.MaxPlayers = 4;
            startButton.SetActive(false);
        }

        PV.RPC("RPC_SendItemInfo", RpcTarget.AllBuffered, acount.playerLevel, acount.character);
        foreach(Transform child in playerList)
        {
            Destroy(child.gameObject);
        }

        GameObject g = PhotonNetwork.Instantiate(Path.Combine("Player", "VoicePlayer"), Vector3.zero, Quaternion.identity);
        PVV = g.GetComponent<PhotonVoiceView>();

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
        if (PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.PlayerList.Length == 4)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
            }
        }

        RemoveSpawnedCharacters();
        foreach (Transform child in playerList)
        {
            Destroy(child.gameObject);
        }
        buffered = false;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        StartCoroutine(UpdateOnLeft());
    }
    IEnumerator UpdateOnLeft()
    {
        yield return new WaitForSeconds(.1f);
        RemoveSpawnedCharacters();
        foreach (Transform child in playerList)
        {
            Destroy(child.gameObject);
        }
        print("Players: " + PhotonNetwork.PlayerList.Length);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject g = Instantiate(playerItem, playerList);
            PlayerItem PI = g.GetComponent<PlayerItem>();
            if (PhotonNetwork.PlayerList[i].IsMasterClient)
            {
                PI.host.SetActive(true);
            }
            PI.playerName.text = PhotonNetwork.PlayerList[i].NickName;
            StartCoroutine(UpdatePlayerItem(PI, i));
        }
    }
    IEnumerator UpdatePlayerItem(PlayerItem PI, int i)
    {
        yield return new WaitForSeconds(.1f);
        SpawnCharacterPreviews();
        PI.playerLevel.text = lvls[i].ToString();
        PI.UpdateSprite(characters[i]);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void LeaveRoom()
    {
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
        PhotonNetwork.Destroy(PVV.gameObject);
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
    public void SpawnCharacterPreviews()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            Instantiate(CharacterPrefabs[characters[i]], spawnPoints[i]);
        }
    }
    public void RemoveSpawnedCharacters()
    {
        foreach(Transform t in spawnPoints)
        {
            print("Remove");
            foreach(Transform t2 in t)
            {
                Destroy(t2.gameObject);
            }
        }
    }
    public void RoomList()
    {
        MenuSwitcher.Instance.SwitchPanel("roomsList");
    }
    public void Shop()
    {
        MenuSwitcher.Instance.SwitchPanel("shop");
    }
    public void Account()
    {
        MenuSwitcher.Instance.SwitchPanel("account");
    }
    public void Options()
    {
        MenuSwitcher.Instance.SwitchPanel("options");
    }
    public void Updates()
    {
        MenuSwitcher.Instance.SwitchPanel("updates");
    }
    public void Credits()
    {
        MenuSwitcher.Instance.SwitchPanel("credits");
    }
    public void Quit()
    {
        print("Quitting");
        Application.Quit();
    }
}