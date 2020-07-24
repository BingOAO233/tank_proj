using System;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public CameraControl cameraControl;//视角控制
    public TankManager tankManagerPrefab;//坦克控制台预设
    public GameObject tankPrefab;//坦克预设
    public GameObject mapPrefab;//地图预设
    public GameObject mapSpawnPoint;//地图生成点
    public GeneralUI generalUI;//UI
    public Canvas menuCanvas;
    //UI内元素
    public Text screenMessageBox;
    public Text scoreCounter;
    
    public int roundNum;//回合数
    public int startDelay;//开始延时
    public int endDelay;//终局延时
    public int roundDelay;//回合延时
    public int totalPlayerNum;//总玩家人数
    

    [HideInInspector] public TankManager[] tanks;//坦克控制台
    [HideInInspector] public MapController mapController;//地图元素控制器
    [HideInInspector] public TankManager winnerTank;//胜出坦克

    private GameObject m_Map;//地图对象
    private WaitForSeconds m_WaitForSecondsUntilStart;//延时
    private WaitForSeconds m_WaitForSecondsAfterEnd;//延时
    private WaitForSeconds m_WaitForSecondsBetweenRounds;//延时
    private TankManager m_RoundWinnerTank;//回合胜出者
    private bool m_HaveAWinner;//是否产生胜者
    private int m_RoundNum;//当前回合数
    
    public void Start()
    {
        //初始化时间延时
        m_WaitForSecondsUntilStart = new WaitForSeconds(startDelay);
        m_WaitForSecondsAfterEnd = new WaitForSeconds(endDelay);
        m_WaitForSecondsBetweenRounds = new WaitForSeconds(roundDelay);
        
        //初始化参数
        m_RoundWinnerTank = null;
        m_HaveAWinner = false;
        winnerTank = null;
        m_RoundNum = 0;
        m_HaveAWinner = false;
    }

    public void GameStartSetup()
    {
        //TankManager预设生成并导入tanks
        tanks = new TankManager[totalPlayerNum];
        for (int i = 0; i < totalPlayerNum; i++)
        {
            tanks[i] = Instantiate(tankManagerPrefab, transform)as TankManager;
            tanks[i].playerColor = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),1f);
        }

        //进入游戏循环
        StartCoroutine(GameLoop());
        //throw new NotImplementedException();
    }
    /*
    //地图预设生成并初始化地图元素控制器
    m_MapPrefab = mapPrefabs[chosenMap];
    m_Map = Instantiate(m_MapPrefab, mapSpawnPoint.transform);
    mapController = m_Map.GetComponent<MapController>();
        
    //生成全体坦克副本
    SpawnAllTanks();
        
    //初始化镜头
    CameraSetup();
    */
    private void CameraSetup()//提取camera的取景目标
    {
        generalUI.resetTransform();
        Transform[] targets = new Transform[tanks.Length];
        
        //逐项提取tank的transform
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = tanks[i].instance.transform;
        }
        cameraControl.targets = targets;
        
        generalUI.gameObject.SetActive(false);
    }
    
    private void SpawnAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].spawnPoint = mapController.spawnPoints[i].transform;
            tanks[i].instance =
                Instantiate(tankPrefab, tanks[i].spawnPoint.position, tanks[i].spawnPoint.rotation) as GameObject;
            tanks[i].playerNumber = i + 1;
            tanks[i].Setup();
        }
    }

    private void CameraReset()
    {
        Transform[] targets = new Transform[0];
        cameraControl.targets = targets;
        generalUI.gameObject.SetActive(true);
    }
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(GameStart());
        while(!m_HaveAWinner)
        {
            yield return StartCoroutine(RoundStart());
            yield return StartCoroutine(RoundPlaying());
            yield return StartCoroutine(RoundEnd());
        }
        yield return StartCoroutine(GameEnd());
    }

    private IEnumerator GameStart()
    {
        //隐藏菜单
        menuCanvas.gameObject.SetActive(false);

        //初始化参数
        m_HaveAWinner = false;
        m_RoundNum = 0;

        //地图预设生成并初始化地图元素控制器
        m_Map = Instantiate(mapPrefab, mapSpawnPoint.transform);
        mapController = m_Map.GetComponent<MapController>();
        
        //生成全体坦克副本
        SpawnAllTanks();
        
        //初始化镜头
        CameraSetup();
        //cameraControl.SetStartPositionAndSize();
        
        //初始化回合计数器
        string message = "<color=#" + ColorUtility.ToHtmlStringRGB(tanks[0].playerColor) + ">Player1 ===>  " +
                         tanks[0].winRoundNum + " </color> Vs.  <color=#" +
                         ColorUtility.ToHtmlStringRGB(tanks[1].playerColor) + ">" + tanks[1].winRoundNum +
                         "  <=== Player2</color>";
        DisplayMessage(scoreCounter,message);
        
        //返回进程
        yield return null;
    }
    
    private IEnumerator GameEnd()
    {
        //回收生成对象
        for (int i = 0; i < tanks.Length; i++)
        {
            Destroy(tanks[i].instance);
            Destroy(tanks[i]);
        }
        Destroy(m_Map);

        //重置回开始页面
        CameraReset();
        menuCanvas.gameObject.SetActive(true);
        DisplayMessage(screenMessageBox,"1 vs. 1");
        DisplayMessage(scoreCounter,null);
        yield return null;
    }

    private IEnumerator  RoundStart()
    {
        //回合初重置坦克
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].ResetTank();
            tanks[i].DisableControl();
            tanks[i].instance.SetActive(true);
        }
        //回合初调整回合数并显示
        m_RoundNum++;
        string message = "Round " + m_RoundNum;
        DisplayMessage(screenMessageBox,message);
        yield return new WaitForSeconds(1);
        DisplayCountDown(startDelay);
        
        //计时开始
        yield return m_WaitForSecondsUntilStart;
        
        //清空显示文字
        DisplayMessage(screenMessageBox,null);
        
    }

    private IEnumerator RoundPlaying()
    {
        //回合开始解除锁定
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].EnableControl();  
        }
        
        //每帧监测存活坦克数
        while(LastTankNum()>=2)
        {
            yield return null;
        }
    }
    
    private IEnumerator RoundEnd()
    {
        m_RoundWinnerTank = GetRoundWinner();
        if (m_RoundWinnerTank != null)
        {
            m_RoundWinnerTank.winRoundNum++;
            string message = "<color=#" + ColorUtility.ToHtmlStringRGB(tanks[0].playerColor) + ">Player1 ===>  " +
                             tanks[0].winRoundNum + " </color> Vs.  <color=#" +
                             ColorUtility.ToHtmlStringRGB(tanks[1].playerColor) + ">" + tanks[1].winRoundNum +
                             "  <=== Player2</color>";
            DisplayMessage(scoreCounter,message);
            if (m_RoundWinnerTank.winRoundNum >= roundNum / 2 + 1)
            {
                winnerTank = m_RoundWinnerTank;
                m_HaveAWinner = true;
                DisplayMessage(screenMessageBox,CorrespondingMessage());
                yield return m_WaitForSecondsAfterEnd;
                DisplayMessage(screenMessageBox,null);
            }
            else
            {
                DisplayMessage(screenMessageBox,CorrespondingMessage());
                yield return m_WaitForSecondsBetweenRounds;
                DisplayMessage(screenMessageBox,null);
            }
        }
        else
        {
            //Draw平局   
            DisplayMessage(screenMessageBox,CorrespondingMessage());
            yield return m_WaitForSecondsBetweenRounds;
            DisplayMessage(screenMessageBox,null);
        }
        
        
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].DisableControl();
            tanks[i].instance.SetActive(false);
        }
    }

    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
                return tanks[i];
        }
        return null;
    }

    

    private string CorrespondingMessage()
    {
        string message;
        if (m_HaveAWinner)
        {
            message = "<color=#" + ColorUtility.ToHtmlStringRGB(winnerTank.playerColor) + ">PLAYER" +
                      winnerTank.playerNumber + "</color> is the WINNER !!!";
        }
        else
        {
            message = "! ?  TIE  ? !";
            if (m_RoundWinnerTank != null)
            {
                message = "<color=#" + ColorUtility.ToHtmlStringRGB(m_RoundWinnerTank.playerColor) + ">PLAYER" +
                    m_RoundWinnerTank.playerNumber + "</color> win round " + m_RoundNum + "!!!";
            }
        }
        
        return message;
    }
    
    private int LastTankNum()
    {
        int currentActiveTankNum = 0;
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
            {
                currentActiveTankNum++;
            }
        }
        return currentActiveTankNum;
    }

    private void DisplayMessage(Text displayTarget,string message)
    {
        displayTarget.text = message;
    }

    private void DisplayCountDown(int countDownSecond)
    {
        StartCoroutine(CountDown(countDownSecond));
    }

    private IEnumerator CountDown(int second)
    {
        string message;
        while (second>0)
        {
            message = "" + second;
            DisplayMessage(screenMessageBox,message);
            yield return new WaitForSeconds(1);
            second--;
        }

        message = "GO";
        DisplayMessage(screenMessageBox,message);
        yield return new WaitForSeconds(1);
        DisplayMessage(screenMessageBox,null);
    }
    
}