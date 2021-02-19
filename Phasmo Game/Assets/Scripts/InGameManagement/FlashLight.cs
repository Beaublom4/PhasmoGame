using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : Item
{
    public Light light;
    private void Update()
    {
        if (turnedOn)
            light.enabled = true;
        else
            light.enabled = false;
    }
}
