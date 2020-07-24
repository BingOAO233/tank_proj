using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public Transform[] fireshellTransform;
    public AudioSource fireAudio;
    public Weapon tankWeapon;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public Rigidbody[] shellRigidbody;
    [HideInInspector] public int shellCount;


    protected string m_FireButton;
    protected bool m_IsFired;
    protected float m_CurrentReloadTime;
    protected float m_FlagTime;

    private void Start()
    {
        shellRigidbody = new Rigidbody[10];
        m_FireButton = "Fire" + playerNumber;
        m_FlagTime = Time.time;
        shellCount = 0;
        //throw new NotImplementedException();
    }

    private void Update()
    {
        m_CurrentReloadTime = Time.time - m_FlagTime;//计算距上次开火时间
        if (m_CurrentReloadTime >= tankWeapon.reloadTime && !m_IsFired && Input.GetButton(m_FireButton))
        {
            Fire();
        }
        else
        {
            m_IsFired = false;
        }
        //throw new NotImplementedException();
    }

    private void Fire()
    {
        m_IsFired = true;

        if (shellRigidbody[shellCount] != null)
            DestroyImmediate(shellRigidbody[shellCount]);
        int transformIndex = new System.Random().Next(fireshellTransform.Length);
        shellRigidbody[shellCount] = Instantiate(tankWeapon.shellprefab, fireshellTransform[transformIndex].position, fireshellTransform[transformIndex].rotation) as Rigidbody;
        shellRigidbody[shellCount].velocity = tankWeapon.fireForce * fireshellTransform[transformIndex].forward;
        shellCount ++;
        if (shellCount >= 10)
            shellCount = 0;
        fireAudio.Play();

        m_FlagTime = Time.time;
    }
}
