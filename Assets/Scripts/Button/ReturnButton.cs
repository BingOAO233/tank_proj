using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ReturnButton : MonoBehaviour
{
    public Canvas menuCanvas;
    public Canvas configCanvas;
    
    public void Click()//选项界面
    {
        configCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(true);
    }
}
