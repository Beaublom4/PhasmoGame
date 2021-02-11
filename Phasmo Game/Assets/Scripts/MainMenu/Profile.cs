using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Profile : MonoBehaviour
{
	public int playerLv;
	public Sprite playerSprite;

	[PunRPC]
	public void SettingProfile()
    {

    }
}