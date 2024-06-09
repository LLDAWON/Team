using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light lightSource;
    public KeyCode switchKey = KeyCode.Tab;

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            lightSource.enabled = !lightSource.enabled;
        }
    }
}
