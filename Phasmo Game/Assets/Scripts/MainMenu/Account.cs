using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class Account : MonoBehaviour
{
    public TMP_Text playerName, playerLv, playerLvXp;
    public Slider lvSlider;

    public Acount acountScript;

    private void OnEnable()
    {
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        playerLv.text = "LV:" + acountScript.playerLevel.ToString();
        playerLvXp.text = acountScript.xp.ToString() + "/" + acountScript.wantedXP.ToString();
        lvSlider.maxValue = acountScript.wantedXP;
        lvSlider.value = acountScript.xp;
    }
}
