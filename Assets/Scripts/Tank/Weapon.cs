using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public Rigidbody shellprefab;
    public AudioSource fireAudio;
    [HideInInspector] public float reloadTime;
    [HideInInspector] public float fireForce;
    [HideInInspector] public bool isAutoFire;
}
