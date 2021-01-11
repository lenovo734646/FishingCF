
/******************************************************************************
 * 
 *  Title:  捕鱼项目
 *
 *  Version:  1.0版
 *
 *  Description:
 *         1：所有UI窗体的父类
 *
 *  Author:  WangXingXing
 *       
 *  Date:  2018
 * 
 ******************************************************************************/

using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;

public abstract class BaseUI : MonoBehaviour
{

    protected virtual int UIOrder { get { return -1; } }
    protected virtual EnumAnimationType AnimType { get { return EnumAnimationType.Scale; } }
    public virtual bool EscapeClose { get { return true; } }

    private Tweener tweener;
    private float tweenerTime = 0.3f;

    public abstract EnumUIType GetUIType();

    public string UIName = string.Empty;

    private GameObject cacheGameObjet;
    public GameObject CacheGameObject
    {
        get
        {
            if (null == cacheGameObjet)
                cacheGameObjet = gameObject;
            return cacheGameObjet;
        }
    }

    private Transform cacheTransform;
    public Transform CacheTransform
    {
        get
        {
            if (null == cacheTransform)
                cacheTransform = transform;
            return cacheTransform;
        }
    }

    protected EnumObjectState state = EnumObjectState.None;
    public event StateChangeEvent StateChanged;
    public EnumObjectState State
    {
        get { return state; }
        protected set
        {
            if (value != state)
            {
                EnumObjectState oldState = state;
                state = value;
                StateChanged?.Invoke(this, state, oldState);
            }
        }
    }

    public void Awake()
    {
        setAnimation();
        State = EnumObjectState.Initial;
        OnAwake();
        State = EnumObjectState.Loading;
        OnPlayOpenUIAudio();
    }
    private void Start()
    {
        if (UIOrder > -1)
        {
            var canvas = this.GetOrAddComponent<Canvas>();
            this.GetOrAddComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = UIOrder;
        }
        var btnRelease = UnityHelper.FindTheChild(gameObject, "btnRelease");
        if (btnRelease)
            EventTriggerListener.Get(btnRelease).SetEventHandle(EnumTouchEventType.OnClick, OnBtnRelease,
                UnityHelper.CreateHashtable(EnumHashtableParamsType.LockAllClick, ""));
        playAnimation(true);
        OnStart();
    }

    protected virtual void OnBtnRelease(GameObject gameObject, object eventData, params object[] args)
    {
        CloseUI();
    }

    protected void CloseUI()
    {
        if (GetUIType() != EnumUIType.LuaUI)
            UIManager.Instance.CloseUI(GetUIType());
        else
            UIManager.Instance.CloseLuaUIObject(UIName);
    }

    private void Update()
    {
        if (State == EnumObjectState.Ready)
        {
            OnUpdate(Time.deltaTime);
        }
    }
    public void Release()
    {
        State = EnumObjectState.Closing;
        OnRelease();
        playAnimation(false);
    }

    private void OnDestroy()
    {
        State = EnumObjectState.None;
        OnPlayCloseUIAudio();
    }

    //ui层级设置
    protected virtual void SetDepthToTop() { }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }

    protected virtual void OnUpdate(float deltaTime) { }

    protected virtual void OnLoadData() { }

    protected virtual void OnRelease() { }

    protected virtual void OnPlayOpenUIAudio() { }
    protected virtual void OnPlayCloseUIAudio() { }

    protected virtual void SetUI(params object[] uiParams)
    {
        CacheTransform.SetParent(GameController.Instance.UIParent, false);
        State = EnumObjectState.Loading;
    }

    public void SetUIWhenOpening(params object[] uiParams)
    {
        SetUI(uiParams);
        StartCoroutine(LoadDataAsyn());
    }

    private IEnumerator LoadDataAsyn()
    {
        yield return new WaitForSeconds(0f);
        if (State == EnumObjectState.Loading)
        {
            OnLoadData();
            State = EnumObjectState.Ready;
        }
    }

    public Transform Find(string name)
    {
        return CacheTransform.Find(name);
    }

    private void setAnimation()
    {
        if (AnimType == EnumAnimationType.None) return;
        switch (AnimType)
        {
            case EnumAnimationType.Scale:
                tweener = CacheTransform.DOScale(Vector3.one * 1.2f, tweenerTime).From().SetEase(Ease.OutQuart);
                break;
        }
        tweener.OnRewind(() => { Destroy(CacheGameObject); });
        tweener.SetAutoKill(false);
        tweener.Pause();
    }

    private void playAnimation(bool isOpen)
    {
        if (AnimType == EnumAnimationType.None)
        {
            if (!isOpen)
                Destroy(CacheGameObject);
        }else
        {
            if (isOpen)
                tweener.PlayForward();
            else
                tweener.PlayBackwards();
        }
    }
}