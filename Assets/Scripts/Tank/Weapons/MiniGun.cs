using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class MiniGun : Weapon
{
    private void Start()
    {
        reloadTime = 0.15f;
        fireForce = 80;
        isAutoFire = true;
    }
}
