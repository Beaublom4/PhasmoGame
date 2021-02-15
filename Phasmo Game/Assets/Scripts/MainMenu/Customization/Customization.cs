using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customization : MonoBehaviour
{
    [SerializeField] Acount acount;
    public void ChangeCharacter(Character character)
    {
        acount.character = character.ID;
    }
}
