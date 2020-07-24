using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameStartButton : MonoBehaviour
{
    public GameManager gameManager;
    
    public void Click()//选项界面
    {
        gameManager.GameStartSetup();
    }
}
