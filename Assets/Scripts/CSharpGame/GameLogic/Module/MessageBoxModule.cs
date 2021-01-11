
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

using System;
using System.Collections;
using UnityEngine;

public class MessageBoxModule : BaseModule
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int CountTime { get; set; }
    public EnumMessageBoxType MessageType { get; set; }

    public MethodAction btnOK;
    public MethodAction btnRelease;

    public object btnOKParam;
    public object btnReleaseParam;

    public MessageBoxModule()
    {
        AutoRegister = true;
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="btnNum">按钮类型</param>
    public void Send(bool isConfirm)
    {
        switch (MessageType)
        {
            case EnumMessageBoxType.OK:
                btnOK?.Invoke(btnOKParam);
                btnOK = null;
                break;
            case EnumMessageBoxType.OK_CANCEL:
                if (isConfirm)
                    btnOK?.Invoke(btnOKParam);
                else
                    btnRelease?.Invoke(btnReleaseParam);
                break;
        }
        btnOK = null;
        btnRelease = null;
        btnOKParam = null;
        btnRelease = null;
    }
}