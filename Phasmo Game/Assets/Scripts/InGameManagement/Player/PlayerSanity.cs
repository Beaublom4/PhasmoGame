using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSanity : MonoBehaviour
{
    public float sanityMeter = 100; //overall sanity
    public float currentDecrease; //when in "Specific Room" set this to -= amound

    private void Update()
    {
        if(currentDecrease > 0)
        {
            DecreseSanity(currentDecrease);
        }
    }
    public void DecreseSanity(float amount) //decrease in amounds
    {
        sanityMeter -= amount;
        sanityMeter = Mathf.Clamp(sanityMeter, 0, 100);
    }
}
