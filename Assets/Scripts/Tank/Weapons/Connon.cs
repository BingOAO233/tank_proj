using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class Connon : Weapon
{
    private void Start()
    {
        reloadTime = 1.5f;
        fireForce = 50;
        isAutoFire = false;
    }
}
