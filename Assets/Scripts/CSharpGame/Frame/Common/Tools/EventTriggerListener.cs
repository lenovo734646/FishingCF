
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

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHandle
{
    private event OnTouchEventHandle eventHandle = null;
    private object[] handleargs;
    private Hashtable hashtable;
    private float waitTime = 0.5f;

    public TouchHandle() {  }
    public TouchHandle(OnTouchEventHandle handle, Hashtable hashtable, params object[] args)
    {
        SetHandle(handle, hashtable, args);
    }

    public void SetHandle(OnTouchEventHandle handle, Hashtable hashtable, params object[] args)
    {
        DestroyHandle();
        this.hashtable = hashtable;
        eventHandle += handle;
        handleargs = args;
    }

    public void CallEventHandle(GameObject listener, object args)
    {
        var eventTrigger = listener.GetComponent<EventTriggerListener>();
        var audioName = string.Empty;
        if (!ReferenceEquals(hashtable, null))
        {
            foreach (EnumHashtableParamsType type in hashtable.Keys)
            {
                switch (type)
                {
                    case EnumHashtableParamsType.Audio:
                        audioName = hashtable[type].ToString();
                        break;
                    case EnumHashtableParamsType.LockAllClick:
                        var strAllTime = hashtable[EnumHashtableParamsType.LockAllClick].ToString();
                        waitTime = string.IsNullOrEmpty(strAllTime) ? waitTime : float.Parse(strAllTime);
                        CoroutineController.Instance.StartCoroutine(lockAllClick());
                        break;
                    case EnumHashtableParamsType.LockSelfClick:
                        var strSelfTime = hashtable[EnumHashtableParamsType.LockSelfClick].ToString();
                        waitTime = string.IsNullOrEmpty(strSelfTime) ? waitTime : float.Parse(strSelfTime);
                        CoroutineController.Instance.StartCoroutine(lockSelfClick(eventTrigger));
                        break;
                }
            }
        }
        audioName = string.IsNullOrEmpty(audioName) ? "btn1" : audioName;
        AudioManager.Instance.PlaySoundEff2D(string.Format($"{SysDefines.AUDIO}{audioName}"));
        eventHandle?.Invoke(listener, args, handleargs);
    }

    private IEnumerator lockAllClick()
    {
        EventTriggerListener.isLockAllClick = true;
        yield return new WaitForSeconds(waitTime);
        EventTriggerListener.isLockAllClick = false;
    }

    private IEnumerator lockSelfClick(EventTriggerListener eventTrigger)
    {
        eventTrigger.islockSelfClick = true;
        yield return new WaitForSeconds(waitTime);
        eventTrigger.islockSelfClick = false;
    }

    public void DestroyHandle()
    {
        eventHandle = null;
    }

}

public class EventTriggerListener : EventTrigger
{
    private TouchHandle onBeginDrag;
    private TouchHandle onCancel;
    private TouchHandle onDeselect;
    private TouchHandle onDrag;
    private TouchHandle onDrop;
    private TouchHandle onEndDrag;
    private TouchHandle onInitializePotentialDrag;
    private TouchHandle onMove;
    private TouchHandle onClick;
    private TouchHandle onDoubleClick;
    private TouchHandle onDown;
    private TouchHandle onEnter;
    private TouchHandle onExit;
    private TouchHandle onUp;
    private TouchHandle onScroll;
    private TouchHandle onSelect;
    private TouchHandle onSubmit;
    private TouchHandle onUpdateSelected;

    public bool IngnoreClick = false;                                //拖拽不响应Click


    static public EventTriggerListener Get(UnityEngine.UI.Button btn)
    {
        return Get(btn.gameObject);
    }

    static public EventTriggerListener Get(Transform transform)
    {
        return Get(transform.gameObject);
    }

    static public EventTriggerListener Get(GameObject go)
    {
        return go.GetOrAddComponent<EventTriggerListener>();
    }

    /// <summary>
    /// 设置添加的事件
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="handle">事件的委托</param>
    /// <param name="hashtable">事件的hashtable</param>
    /// <param name="args">可变参数</param>
    public void SetEventHandle(EnumTouchEventType type, OnTouchEventHandle handle, Hashtable hashtable = null, params object[] args)
    {
        switch (type)
        {
            case EnumTouchEventType.OnBeginDrag:
                if (null == onBeginDrag)
                    onBeginDrag = new TouchHandle();
                onBeginDrag.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnCancel:
                if (null == onCancel)
                    onCancel = new TouchHandle();
                onCancel.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnDeselect:
                if (null == onDeselect)
                    onDeselect = new TouchHandle();
                onDeselect.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnDrag:
                if (null == onDrag)
                    onDrag = new TouchHandle();
                onDrag.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnDrop:
                if (null == onDrop)
                    onDrop = new TouchHandle();
                onDrop.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnEndDrag:
                if (null == onEndDrag)
                    onEndDrag = new TouchHandle();
                onEndDrag.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnInitializePotentialDrag:
                if (null == onInitializePotentialDrag)
                    onInitializePotentialDrag = new TouchHandle();
                onInitializePotentialDrag.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnMove:
                if (null == onMove)
                    onMove = new TouchHandle();
                onMove.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnClick:
                if (null == onClick)
                    onClick = new TouchHandle();
                onClick.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnDoubleClick:
                if (null == onDoubleClick)
                    onDoubleClick = new TouchHandle();
                onDoubleClick.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnDown:
                if (null == onDown)
                    onDown = new TouchHandle();
                onDown.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnEnter:
                if (null == onEnter)
                    onEnter = new TouchHandle();
                onEnter.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnExit:
                if (null == onExit)
                    onExit = new TouchHandle();
                onExit.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnUp:
                if (null == onUp)
                    onUp = new TouchHandle();
                onUp.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnScroll:
                if (null == onScroll)
                    onScroll = new TouchHandle();
                onScroll.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnSelect:
                if (null == onSelect)
                    onSelect = new TouchHandle();
                onSelect.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnSubmit:
                if (null == onSubmit)
                    onSubmit = new TouchHandle();
                onSubmit.SetHandle(handle, hashtable, args);
                break;
            case EnumTouchEventType.OnUpdateSelected:
                if (null == onUpdateSelected)
                    onUpdateSelected = new TouchHandle();
                onUpdateSelected.SetHandle(handle, hashtable, args);
                break;
        }
    }

    private void RemoveAllHandle()
    {

        RemoveHandle(onBeginDrag);
        RemoveHandle(onCancel);
        RemoveHandle(onDeselect);
        RemoveHandle(onDrag);
        RemoveHandle(onDrop);
        RemoveHandle(onEndDrag);
        RemoveHandle(onInitializePotentialDrag);
        RemoveHandle(onMove);
        RemoveHandle(onClick);
        RemoveHandle(onDoubleClick);
        RemoveHandle(onDown);
        RemoveHandle(onEnter);
        RemoveHandle(onExit);
        RemoveHandle(onUp);
        RemoveHandle(onScroll);
        RemoveHandle(onSelect);
        RemoveHandle(onSubmit);
        RemoveHandle(onUpdateSelected);
    }

    private void RemoveHandle(TouchHandle handle)
    {
        if (null != handle)
        {
            handle.DestroyHandle();
            handle = null;
        }
    }

    private void OnDestroy()
    {
        RemoveAllHandle();
    }

    public static bool isLockAllClick;
    public bool islockSelfClick;

    //是否可点击
    private bool canCallClick()
    {
        if (!enabled || isLockAllClick || islockSelfClick) return false;
        return true;
    }

    #region 重写事件系统
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (null != onBeginDrag)
            onBeginDrag.CallEventHandle(gameObject, eventData);
    }

    public override void OnCancel(BaseEventData eventData)
    {
        if (null != onCancel)
            onCancel.CallEventHandle(gameObject, eventData);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (null != onDeselect)
            onDeselect.CallEventHandle(gameObject, eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (null != onDrag)
            onDrag.CallEventHandle(gameObject, eventData);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (null != onDrop)
            onDrop.CallEventHandle(gameObject, eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (null != onEndDrag)
            onEndDrag.CallEventHandle(gameObject, eventData);
    }

    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (null != onInitializePotentialDrag)
            onInitializePotentialDrag.CallEventHandle(gameObject, eventData);
    }

    public override void OnMove(AxisEventData eventData)
    {
        if (null != onMove)
            onMove.CallEventHandle(gameObject, eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (canCallClick())
        {
            if (null != onClick && !IngnoreClick)
                onClick.CallEventHandle(gameObject, eventData);
            if (null != onDoubleClick && 2 == eventData.clickCount)
                onDoubleClick.CallEventHandle(gameObject, eventData);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (null != onDown)
            onDown.CallEventHandle(gameObject, eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (null != onEnter)
            onEnter.CallEventHandle(gameObject, eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (null != onExit)
            onExit.CallEventHandle(gameObject, eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (null != onUp)
            onUp.CallEventHandle(gameObject, eventData);
    }

    public override void OnScroll(PointerEventData eventData)
    {
        if (null != onScroll)
            onScroll.CallEventHandle(gameObject, eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (null != onSelect)
            onSelect.CallEventHandle(gameObject, eventData);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        if (null != onSubmit)
            onSubmit.CallEventHandle(gameObject, eventData);
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (null != onUpdateSelected)
            onUpdateSelected.CallEventHandle(gameObject, eventData);
    }
    #endregion
}