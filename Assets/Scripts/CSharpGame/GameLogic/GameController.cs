
/******************************************************************************
 * 
 *  Title:  捕鱼项目
 *
 *  Version:  1.0版
 *
 *  Description:
 *
 *  Author:  WangXingXing
 *       
 *  Date:  2018
 * 
 ******************************************************************************/

using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : DDOLSingleton<GameController>
{
    private RectTransform thisT;
    public RectTransform ThisT
    {
        get
        {
            if (ReferenceEquals(thisT, null))
                thisT = transform as RectTransform;
            return thisT;
        }
    }

    private RectTransform uiParent;
    public RectTransform UIParent
    {
        get
        {
            if (ReferenceEquals(uiParent, null))
                uiParent = UnityHelper.GetTheChildComponent<RectTransform>(gameObject, "UIParent");
            return uiParent;
        }
    }

    private RectTransform uiEffect;
    public RectTransform UIEffect
    {
        get
        {
            if (ReferenceEquals(uiEffect, null))
                uiEffect = UnityHelper.GetTheChildComponent<RectTransform>(gameObject, "UIEffect");
            return uiEffect;
        }
    }

    private RectTransform hintMessage;
    public RectTransform HintMessage
    {
        get
        {
            if (ReferenceEquals(hintMessage, null))
                hintMessage = UnityHelper.GetTheChildComponent<RectTransform>(gameObject, "HintMessage");
            return hintMessage;
        }
    }

    private Canvas mainCanvas;
    public Canvas MainCanvas
    {
        get
        {
            if (null == mainCanvas)
                mainCanvas = ThisT.GetComponent<Canvas>();
            return mainCanvas;
        }
    }

    private CanvasScaler mainCanvasScaler;
    public CanvasScaler MainCanvasScaler
    {
        get
        {
            if (null == mainCanvasScaler)
                mainCanvasScaler = ThisT.GetComponent<CanvasScaler>();
            return mainCanvasScaler;
        }
    }

    private Camera mainCamera;
    public Camera MainCamera
    {
        get
        {
            if (null == mainCamera)
                mainCamera = UnityHelper.GetTheChildComponent<Camera>(gameObject, "MainCamera");
            return Camera.main;
        }
    }

    private DOTweenAnimation cameraAnim;
    public DOTweenAnimation CameraAnim
    {
        get
        {
            if (null == cameraAnim)
                cameraAnim = UnityHelper.GetTheChildComponent<DOTweenAnimation>(gameObject, "MainCamera");
            return cameraAnim;
        }
    }

    //暂时放这
    public GamePlayer Player;

    public override void Init()
    {
        getRunPlatform();
        Player = null;
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = true;
    }

    private void Start()
    {
//#if LOCAL_DEBUG
//        Debug.unityLogger.logEnabled = true;
//#else
//        Debug.unityLogger.logEnabled = false;
//#endif
//        m_LastUpdateShowTime = Time.realtimeSinceStartup;
//        TableManager.InitData();
//        ModuleManager.Instance.RegisterAllModules();
//        UIManager.Instance.PreloadUI(SysDefines.preloadUIArray);
//        Screen.sleepTimeout = SleepTimeout.NeverSleep;
//        ThisT.localPosition = new Vector3(0, 0, MainCanvas.planeDistance);

//        UIManager.Instance.OpenUI(EnumUIType.HotUpdateUI);
//        NetController.Instance.GetDownloadUrl((md5) => 
//        {
//            TableLoadHelper.LoadFromNet(SysDefines.OssUrl, md5, () => 
//            {
//                var hotUpdate = THotUpdateHelper.DataMap.Values.Where(t => t.GameId == SysDefines.GameID_Hall).Last();
//                SysDefines.HotUpdateUrl = hotUpdate.Url;
//                MessageCenter.Instance.SendMessage(MsgType.CLIENT_JSON_UPDATE_ACK, this, null);
//            });
//        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var curUIType = UIManager.Instance.GetCurrentUI();
            if (curUIType != EnumUIType.None && curUIType != EnumUIType.LoadingUI)
            {
                var curBaseUI = UIManager.Instance.GetUI<BaseUI>(curUIType);
                if (curBaseUI.EscapeClose)
                    UIManager.Instance.CloseUI(curUIType);
                else
                {
                    var sceneName = (EnumSceneType)Enum.Parse(typeof(EnumSceneType), SceneManager.GetActiveScene().name, true);
                    switch (sceneName)
                    {
                        case EnumSceneType.LoginScene:
                            QuitGame();
                            break;
                        case EnumSceneType.LoadingScene:
                            break;
                        case EnumSceneType.MainScene:
                            QuitScene(SysDefines.QuitMainScene);
                            break;
                        case EnumSceneType.FishScene:
                            QuitScene(SysDefines.QuitFishScene);
                            break;
                    }
                }
            }
        }

        m_FrameUpdate++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }
    }

    #region public function
    //相机震动
    public void ShakeCamera()
    {
        MainCamera.DOShakePosition(1,50);
    }

    //赏金1 高倍随机倍2 弹头鱼3
    public void ShakeCamera(int power)
    {
        switch (power)
        {
            case 1:
                CameraAnim.duration = 0.6f;
                CameraAnim.endValueV3 = Vector3.one * 20;
                CameraAnim.optionalInt0 = 15;
                CameraAnim.DORewind();
                CameraAnim.DOPlay();
                break;
            case 2:
                CameraAnim.duration = 0.8f;
                CameraAnim.endValueV3 = Vector3.one * 30;
                CameraAnim.optionalInt0 = 15;
                CameraAnim.DORewind();
                CameraAnim.DOPlay();
                break;
            case 3:
                CameraAnim.duration = 1f;
                CameraAnim.endValueV3 = Vector3.one * 50;
                CameraAnim.optionalInt0 = 20;
                CameraAnim.DORewind();
                CameraAnim.DOPlay();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 返回到登录界面
    /// </summary>
    public void ReturnToLogin(object agrs = null)
    {
        UIManager.Instance.OpenUI(EnumUIType.LoadingUI, true, EnumUIType.LoginUI, EnumSceneType.LoginScene);
    }

    /// <summary>
    /// 离开当前场景
    /// </summary>
    /// <param name="content">提示框内容</param>
    public void QuitScene(string content)
    {
        if (content == SysDefines.QuitFishScene)
            UIManager.Instance.OpenUI(EnumUIType.MessageLeaveFishUI);
        else
            UIManager.Instance.OpenMessageBoxUI(content, 10, EnumMessageBoxType.OK_CANCEL, doQuitScene);
    }

    public void DoQuitScene()
    {
        doQuitScene(null);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public void QuitGame()
    {
        UIManager.Instance.OpenMessageBoxUI(SysDefines.QuitGame, 10, EnumMessageBoxType.OK_CANCEL, doQuitGame);
    }

    /// <summary>
    /// 微信授权
    /// </summary>
    public void WechatAuth(string code)
    {
        MessageCenter.Instance.SendMessage(MsgType.CLIENT_AUTHORIZE_ACK, this, null, new Dictionary<string, object>()
        {
            { "platform", 4 },
            { "code", code },
        });
    }

    //提示文本
    private Queue hintMsgQueue = new Queue();
    private bool isHintMsgShowing = false;
    public void CreateHintMessage(string content)
    {
        if (string.IsNullOrEmpty(content)) return;
        hintMsgQueue.Enqueue(content);
        if (!isHintMsgShowing) showHintMessage();
    }

    private void showHintMessage()
    {
        var msg = hintMsgQueue.Dequeue().ToString();
		Debug.Log(msg);
        ResManager.Instance.LoadAsync<GameObject>(string.Format($"{SysDefines.UICONTROLSPREFAB}popupTipsUI"), prefab =>
        {
            var obj = ObjectPoolManager.Instance.Spawn(prefab, HintMessage);
            obj.GetOrAddComponent<HintMessage>().SetHintContent(msg, onHintMessageEnd);
            isHintMsgShowing = true;
        });
    }

    private void onHintMessageEnd()
    {
        if (hintMsgQueue.Count > 0)
            showHintMessage();
        else
            isHintMsgShowing = false;
    }
    #endregion

    #region private function
    /// <summary>
    /// 获取当前运行平台
    /// </summary>
    private void getRunPlatform()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                SysDefines.Platform = 1;
                break;
            case RuntimePlatform.Android:
                SysDefines.Platform = 2;
                break;
            case RuntimePlatform.WindowsPlayer:
                SysDefines.Platform = 3;
                break;
            case RuntimePlatform.LinuxPlayer:
                SysDefines.Platform = 4;
                break;
            case RuntimePlatform.OSXPlayer:
                SysDefines.Platform = 5;
                break;
            default:
                SysDefines.Platform = 3;//测试阶段 todo
                break;
        }
    }

    private void doQuitScene(object agrs)
    {
        var sceneType = (EnumSceneType)Enum.Parse(typeof(EnumSceneType), SceneManager.GetActiveScene().name, true);
        switch (sceneType)
        {
            case EnumSceneType.LoginScene:
                break;
            case EnumSceneType.LoadingScene:
                break;
            case EnumSceneType.MainScene:
                NetController.Instance.SendLogoutReq();
                //NetController.Instance.SendAccessServiceReq(EnumGameGroupType.Fish, EnumAccessServiceType.QuitGame);
                break;
            case EnumSceneType.FishScene:
                NetController.Instance.SendExitGameReq(() =>
                {
                    NetController.Instance.SendExitSiteReq(SysDefines.SiteId);
                });
                break;
        }
    }

    private void doQuitGame(object agrs)
    {
        Application.Quit();
    }
    #endregion

    #region FPS
    private float m_LastUpdateShowTime = 0f;  //上一次更新帧率的时间;  
    private float m_UpdateShowDeltaTime = 0.1f;//更新帧率的时间间隔;  
    private int m_FrameUpdate = 0;//帧数;
    private float m_FPS = 0;

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2, 30, 300, 30), $"<color=red>FPS:  {m_FPS.ToString("f2")}</color>");
    }
    #endregion
}