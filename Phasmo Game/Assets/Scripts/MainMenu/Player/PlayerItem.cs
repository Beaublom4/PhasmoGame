using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerItem : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text playerLevel;
    public Image character;

    public GameObject host;

    public Sprite[] characterSprites;

    private void Start()
    {
        if(PhotonNetwork.LocalPlayer.NickName == PhotonNetwork.MasterClient.NickName)
        {
            host.SetActive(true);
        }
    }
    public void UpdateSprite(int id)
    {
        character.sprite = characterSprites[id];
    }
}
