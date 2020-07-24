using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ConfigButton : MonoBehaviour
{
    public Canvas menuCanvas;
    public Canvas configCanvas;
    
    public void Click()//选项界面
    {
        menuCanvas.gameObject.SetActive(false);
        configCanvas.gameObject.SetActive(true);
    }
}
