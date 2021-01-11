
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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        if (!SysDefines.IsCheckVersion)
        {
            //初始化UI界面
            var go = Resources.Load<GameObject>(SysDefines.UIPREFAB + "Canvas");
            var copyGo = Instantiate(go, Vector3.zero, Quaternion.identity);
            GameController.go = copyGo;
            GameController.Instance.Init();
            SysDefines.IsCheckVersion = true;//todo
        }
    }
}