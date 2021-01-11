
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

public class CoroutineController : DDOLSingleton<CoroutineController>
{
    private Coroutine aliveCor = null;
    public void StopAliveCor()
    {
        if (aliveCor != null)
        {
            StopCoroutine(aliveCor);
            aliveCor = null;
        }
    }
    public void StartAliveCor()
    {
        aliveCor = StartCoroutine(NetController.Instance.SendTKeepAlive());
    }
}