using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MapChooseButton : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject mapPrefab;

    public void Click()//设置界面
    {
        gameManager.mapPrefab = mapPrefab;
    }
}
