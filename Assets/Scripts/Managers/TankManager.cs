using System;
using UnityEngine;

[Serializable]
public class TankManager : MonoBehaviour
{
    [HideInInspector] public Transform spawnPoint;
    [HideInInspector] public Color playerColor;
    [HideInInspector] public GameObject instance;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public int winRoundNum = 0;
    public MeshRenderer[] tankPartsMeshRenderers;
    
    private TankMovement m_Movement;
    private TankShooting m_Shooting;
        
    public void Setup()//坦克信息初始化
    {
        //获取移动射击对象
        m_Movement = instance.GetComponent<TankMovement>();
        m_Shooting = instance.GetComponent<TankShooting>();
        
        //设置玩家编号
        m_Movement.playerNumber = playerNumber;
        m_Shooting.playerNumber = playerNumber;
        
        //初始化玩家颜色
        tankPartsMeshRenderers = instance.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < tankPartsMeshRenderers.Length; i++)
        {
            tankPartsMeshRenderers[i].material.color = playerColor;
        }
    }

    public void EnableControl()
    {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;
    }

    public void DisableControl()
    {
        m_Movement.enabled = false;
        for(int i=0;i<m_Shooting.shellCount;i++)
        {
            Destroy(m_Shooting.shellRigidbody[i]);
        }
        m_Shooting.enabled = false;
    }

    public void ResetTank()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;
        instance.SetActive(true);
    }

}
