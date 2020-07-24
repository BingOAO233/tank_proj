using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WeaponChooseButton : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject tankPrefab;

    public void Click()//设置界面
    {
        gameManager.tankPrefab = tankPrefab;
    }
}
