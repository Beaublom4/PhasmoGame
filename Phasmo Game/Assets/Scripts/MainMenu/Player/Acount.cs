using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Acount : MonoBehaviour
{
    public string playerName;
    public int playerLevel;
    public int character;

    public MainMenuManager menuManager;

    public float xp, wantedXP;

    public void DoPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Player_Name"))
        {
            print("PlayerPrefs found");
            PhotonNetwork.NickName = PlayerPrefs.GetString("Player_Name");
            SetData();
            menuManager.playerName.text = PlayerPrefs.GetString("Player_Name");
        }
        else
        {
            PlayerPrefs.SetString("Player_Name", playerName);
            PlayerPrefs.SetInt("Player_Level", playerLevel);
            PlayerPrefs.SetFloat("Player_XP", xp);
            PlayerPrefs.SetFloat("Player_Wanted_XP", wantedXP);
            PlayerPrefs.SetInt("Player_Selected_Character", character);
        }
        PlayerPrefs.DeleteAll();
    }
    public void SetData()
    {
        playerName = PhotonNetwork.NickName;
    }
}
