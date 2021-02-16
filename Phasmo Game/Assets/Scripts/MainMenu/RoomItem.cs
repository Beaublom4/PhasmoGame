using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomItem : MonoBehaviour
{
    [SerializeField] TMP_Text name;
    [SerializeField] TMP_Text players;
    public void SetUp(string _name, int _players)
    {
        name.text = _name;
        players.text = _players.ToString();
    }
    public void JoinRoom()
    {
        MenuSwitcher.Instance.SwitchPanel("loading");
        PhotonNetwork.JoinRoom(name.text);
    }
}
