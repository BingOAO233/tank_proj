using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask tankMask;//坦克层
    public ParticleSystem shellExplosionParticle;//爆炸效果
    public AudioSource shellExplosionAudio;//爆炸音源
    public float shellFullDamage;//炮弹满伤
    public float shellMaxFlyTime;//炮弹最大飞行时间
    //public float shellTriggerRidus = 1f;
    public float shellExplosionRidus;//炮弹爆炸半径

    private void Start()
    {
        Destroy(gameObject,shellMaxFlyTime);
        //throw new NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, shellExplosionRidus, tankMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody collideTarget = colliders[i].GetComponent<Rigidbody>();
            if(!collideTarget)
            {
                continue;
            }

            for (int j = 0; j < colliders.Length; j++)
            {
                TankHealth tankHealth = colliders[j].GetComponent<TankHealth>(); 
                if (!tankHealth)
                { 
                    continue;
                }
                float damage = DamageCounter(colliders[j].GetComponent<Rigidbody>().position);
                tankHealth.HealthDown(damage);
            }
            ShellExplode();
        }
        //throw new NotImplementedException();
    }

    private void Update()
    {
        if (transform.position.y <= 0f)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, shellExplosionRidus, tankMask);
            for (int j = 0; j < colliders.Length; j++)
            {
                TankHealth tankHealth = colliders[j].GetComponent<TankHealth>(); 
                if (!tankHealth)
                { 
                    continue;
                }
                float damage = DamageCounter(colliders[j].GetComponent<Rigidbody>().position);
                tankHealth.HealthDown(damage);
            }
            ShellExplode();
        }
        //throw new NotImplementedException();
    }

    private float DamageCounter(Vector3 targetPosition)
    {
        Vector3 shellToTarget = targetPosition - transform.position;

        float explosionDistance = shellToTarget.magnitude;
        float relativeDistance = (shellExplosionRidus - explosionDistance) / shellExplosionRidus;
        float damage = relativeDistance * shellFullDamage;

        return (Mathf.Max(0f, damage));
    }

    private void ShellExplode()
    {    
        //断开粒子和shell的位置关联
        shellExplosionParticle.transform.parent = null;
        //播放 爆炸音效 和 粒子效果
        shellExplosionAudio.Play();
        shellExplosionParticle.Play();
        //回收粒子 和 shell
        Destroy(shellExplosionParticle.gameObject,shellExplosionParticle.main.duration);
        Destroy(gameObject);
    }
}