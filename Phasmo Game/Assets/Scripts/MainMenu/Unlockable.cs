using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Unlockable : MonoBehaviour
{
    public Acount acountScript;
    public int lv;
    public Image Cover;

    private void OnEnable()
    {
        if (acountScript.playerLevel >= lv)
        {
            Cover.color = new Color(0, 0, 0, 0);
        }
    }
}
