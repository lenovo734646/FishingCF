
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

using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnityHelper
{
    /// <summary>
    /// CA3加密算法：高效快速，自带校验功能
    /// </summary>
    /// <param name="originContent">原始内容</param>
    /// <param name="randumKey">数字随机key</param>
    /// <returns>密文字符串</returns>
    public static string CA3Encode(string originContent, int randumKey)
    {
        byte[] content = System.Text.Encoding.UTF8.GetBytes(originContent);
        byte[] buffer = new byte[content.Length + 4];
        Array.Copy(BitConverter.GetBytes(randumKey), 0, buffer, 0, 4);
        Array.Copy(content, 0, buffer, 4, content.Length);

        int a = 12347, b = 20809, c = 65536;
        for (int i = 0; i < buffer.Length; ++i)
        {
            randumKey = (randumKey * a + b) % c;
            buffer[i] ^= (byte)(randumKey & 0xff);
        }
        return Convert.ToBase64String(buffer);
    }

    /// <summary>
    /// CA3解密算法：高效快速，自带校验功能
    /// </summary>
    /// <param name="encryptContent">密文</param>
    /// <param name="randumKey">数字随机key</param>
    /// <returns>原始内容</returns>
    public static string CA3Decode(string encryptContent, int randumKey)
    {
        byte[] buffer = Convert.FromBase64String(encryptContent);

        if (buffer.Length > 4)
        {
            int tmpKey = randumKey;
            int a = 12347, b = 20809, c = 65536;
            for (int i = 0; i < buffer.Length; ++i)
            {
                randumKey = (randumKey * a + b) % c;
                buffer[i] ^= (byte)(randumKey & 0xff);
            }

            int key = BitConverter.ToInt32(buffer, 0);
            if (key == tmpKey)
                return System.Text.Encoding.UTF8.GetString(buffer, 4, buffer.Length - 4);
        }

        return string.Empty;
    }

    //UrlEncode
    public static string GetUrlEncode(string str)
    {
        StringBuilder sb = new StringBuilder();
        byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str);
        for (int i = 0; i < byStr.Length; i++)
        {
            sb.Append(@"%" + Convert.ToString(byStr[i], 16));
        }
        return (sb.ToString());
    }

    //是否点击在UI控件上，用于判断UI点穿
    public static bool IsPointerOverUIObject()
    {
        if (EventSystem.current == null)
            return false;

        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        var count = results.Count - 1;
        for (int i = count; i >= 0; i--)
        {
            if (results[i].gameObject.layer != 5)
                results.RemoveAt(i);
        }

        return results.Count > 0;
    }

    /// <summary>
    /// 将服务器鱼的坐标转换成游戏坐标
    /// </summary>
    /// <param name="x">服务器点的x的值</param>
    /// <param name="y">服务器点的y的值</param>
    /// <param name="z">服务器点的z的值</param>
    /// <returns></returns>
    public static Vector3 ConvertToVector3(float x,float y,float z)
    {
        //todo 根据游戏屏幕做适配 目前的标准是 1920*1080
        //x = -(9.6f - x * 19.2f);
        //y = -(5.4f - y * 10.8f);
        //z = -(49.0f - z * 98.0f);
        //return new Vector3(x, y, z);
        var rect = (GameController.Instance.transform as RectTransform).rect;
        var widthInSpace = rect.width * 0.01f;
        var heightInSpace = rect.height * 0.01f;
        x = -(widthInSpace * 0.5f - x * widthInSpace);
        y = -(heightInSpace * 0.5f - y * heightInSpace);
        z = -(49.0f - z * 98.0f);
        return new Vector3(x, y, z);
    }
    public static Vector3 FishConvertToVector3(float x, float y, float z)
    {
        //todo 根据游戏屏幕做适配 目前的标准是 1920*1080
        //var anchorX = SeatHelper.IsServerSeatNegative() ? -9.6f : 9.6f;
        //var anchorY = SeatHelper.IsServerSeatNegative() ? -5.4f : 5.4f;
        //x = -(anchorX - x * 19.2f);
        //y = -(anchorY - y * 10.8f);
        //z = -(49.0f - z * 98.0f);
        return (new Vector3(x, y, z)) * 100;
    }

    /// <summary>
    /// 计算两点与X轴的顺时针夹角角度
    /// </summary>
    /// <param name="x1">x1坐标</param>
    /// <param name="y1">y1坐标</param>
    /// <param name="x2">x2坐标</param>
    /// <param name="y2">y2坐标</param>
    /// <returns>向量与X轴顺时针夹角角度</returns>
    public static float CalculateClockwiseAngle(float x1, float y1, float x2, float y2)
    {
        var radian = Mathf.Atan2(y1 - y2, x2 - x1);
        return radian * (180 / Mathf.PI);
    }

    /// <summary>
    /// 计算两点与X轴的逆时针夹角角度
    /// </summary>
    /// <param name="x1">x1坐标</param>
    /// <param name="y1">y1坐标</param>
    /// <param name="x2">x2坐标</param>
    /// <param name="y2">y2坐标</param>
    /// <returns>向量与X轴逆时针夹角角度</returns>
    public static float CalculateAnticlockwiseAngle(float x1, float y1, float x2, float y2)
    {
        var radian = Mathf.Atan2(y2 - y1, x2 - x1);
        return radian * (180 / Mathf.PI);
    }

    /// <summary>
    /// 计算两点间的距离
    /// </summary>
    /// <param name="startPos">起点坐标</param>
    /// <param name="endPos">终点坐标</param>
    /// <returns>距离</returns>
    public static float CalculateDistance(Vector2 startPos, Vector2 endPos)
    {
        var deltaX = endPos.x - startPos.x;
        var deltaY = endPos.y - startPos.y;
        return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    /// <summary>
    /// 将角度修正到0~360范围内
    /// </summary>
    /// <param name="angle">输入角度</param>
    /// <returns>修正后的角度</returns>
    public static float RectifyAngle(float angle)
    {
        var r = angle % 360;
        return r < 0 ? r + 360 : r;
    }

    /// <summary>
    /// 角度转弧度
    /// </summary>
    /// <param name="angle">角度</param>
    /// <returns>弧度</returns>
    public static float AngleToRadian(float angle)
    {
        return (float)(angle / 180 * Math.PI);
    }

    /// <summary>
    /// 根据起点和角度计算终点位置
    /// </summary>
    /// <param name="startX">起点x坐标</param>
    /// <param name="startY">起点y坐标</param>
    /// <param name="length">长度</param>
    /// <param name="angle">与X轴逆时针夹角角度</param>
    /// <returns>终点位置</returns>
    public static Vector2 CalculateDestPointByAngle(Vector2 startPos, float length, float angle)
    {
        var radian = angle / 180 * Math.PI;
        var x = startPos.x + length * (float)Math.Cos(radian);
        var y = startPos.y + length * (float)Math.Sin(radian);
        return new Vector2(x, y);
    }

    /// <summary>
    /// 播放spine2D动画
    /// </summary>
    /// <param name="obj">将要播放动画的物体控件</param>
    /// <param name="anim">动画名称</param>
    /// <param name="loop">是否循环播放</param>
    public static void PlaySpineAnim(GameObject obj, string anim, bool loop)
    {
        SkeletonGraphic graphic = obj.GetComponent<SkeletonGraphic>();
        if (graphic == null)
            return;

        graphic.startingAnimation = anim;
        graphic.startingLoop = loop;
        graphic.Initialize(true);
    }

    /// <summary>
    /// 清除父节点下所有子节点
    /// </summary>
    /// <param name="trans">父节点</param>
    public static void ClearChildren(Transform trans)
    {
        if (null == trans)
            return;
        for (int i = 0; i < trans.childCount; i++)
        {
            UnityEngine.Object.Destroy(trans.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 查找子对象
    /// </summary>
    /// <param name="goParent">父对象</param>
    /// <param name="childName">子对象名称</param>
    /// <returns></returns>
    public static Transform FindTheChild(GameObject goParent, string childName)
    {
        Transform searchTrans = goParent.transform.Find(childName);
        if (ReferenceEquals(searchTrans, null))
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChild(trans.gameObject, childName);
                if (!ReferenceEquals(searchTrans, null))
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }

    /// <summary>
    /// 获取子物体的脚本
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="goParent">父对象</param>
    /// <param name="childName">子对象名称</param>
    /// <returns></returns>
    public static T GetTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        return ReferenceEquals(searchTrans, null) ? null : searchTrans.GetOrAddComponent<T>();
    }

    /// <summary>
    /// 给子物体添加脚本
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="goParent">父对象</param>
    /// <param name="childName">子对象名称</param>
    /// <returns></returns>
    public static T AddTheChildComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTrans = FindTheChild(goParent, childName);
        return ReferenceEquals(searchTrans, null) ? null : searchTrans.GetOrAddComponent<T>();
    }

    /// <summary>
    /// 给子物体添加父对象
    /// </summary>
    /// <param name="parentTrs">父对象的方位</param>
    /// <param name="childTrs">子对象的方位</param>
    public static void AddChildToParent(Transform parentTrs, Transform childTrs)
    {
        childTrs.SetParent(parentTrs, false);
        childTrs.localPosition = Vector3.zero;
        childTrs.localScale = Vector3.one;
        childTrs.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// 复制到剪切板
    /// </summary>
    /// <param name="content">复制的内容</param>
    public static void CopyToClipboard(string content)
    {
        TextEditor te = new TextEditor();
        te.text = new GUIContent(content).text;
        te.SelectAll();
        te.Copy();
    }

    /// <summary>
    /// 用户名显示
    /// </summary>
    /// <param name="nickName">昵称</param>
    /// <param name="length">默认长度12个字符</param>
    /// <returns></returns>
    public static string GetNickNameFormat(string nickName, int length = 12)
    {
        if (string.IsNullOrEmpty(nickName))
            return "";
        int i = 0;//字节数
        int j = 0;//实际截取长度
        foreach (var ch in nickName)
        {
            if (ch > 127)//汉字
                i += 2;
            else
                i++;
            if (i <= length)
                j++;
        }
        return nickName = i <= length ? nickName : nickName.Substring(0, j) + "..";
    }

    /// <summary>
    /// 按汉字占两个字节数获取字符串的长度
    /// </summary>
    /// <param name="content">字符串</param>
    /// <returns>字符串的字节长度</returns>
    public static int GetStringlength(string content)
    {
        var len = 0;
        if (!string.IsNullOrEmpty(content))
        {
            foreach (var ch in content)
            {
                if (ch > 127)
                    len += 2;
                else
                    len++;
            }
        }
        return len;
    }

    /// <summary>
    /// 根据传进来的参数创建一个hashtable
    /// </summary>
    /// <param name="args">成对可变参数</param>
    /// <returns></returns>
    public static Hashtable CreateHashtable(params object[] args)
    {
        Hashtable hashtable = null;
        if (args.Length % 2 == 0)
        {
            hashtable = new Hashtable(args.Length / 2);
            int i = 0;
            while (i < args.Length - 1)
            {
                hashtable.Add(args[i], args[i + 1]);
                i += 2;
            }
        }
        else
            Debug.LogError("Hashtable Error: Hash requires an even number of arguments!");
        return hashtable;
    }

    /// <summary>
    /// 数字格式化
    /// </summary>
    /// <param name="number">原数字</param><param name="dec">返回的字符串小数精度</param>
    public static string NumberFormat(long number, int dec = 2)
    {
        var str = string.Empty;
        var end = string.Empty;

        if (number >= 100000000)
        {
            var num = number * 0.00000001;
            str = $"{string.Format("{0:N" + dec + "}", num)}";
            end = "亿";
        }
        else if (number >= 10000)
        {
            var num = number * 0.0001;
            str = $"{string.Format("{0:N" + dec + "}", num)}";
            end = "万";
        }
        else
        {
            str = number.ToString();
        }
        if (dec > 0 && str.IndexOf(".") != -1)
        {
            var length = str.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                var subChar = str.Substring(i, 1);
                if (subChar == ".")
                {
                    str = str.Substring(0, i);
                    break;
                }
                if (subChar == "0")
                {
                    str = str.Substring(0, i);
                }
                else
                    break;
            }
        }

        return str + end;
    }

    public static string NumberFormatByThousand(long number, int dec = 2)
    {
        var str = string.Empty;
        var end = string.Empty;

        if (number >= 100000000)
        {
            var num = number * 0.00000001;
            str = $"{string.Format("{0:N" + dec + "}", num)}";
            end = "亿";
        }
        else if (number >= 10000)
        {
            var num = number * 0.0001;
            str = $"{string.Format("{0:N" + dec + "}", num)}";
            end = "万";
        }
        else if (number >= 1000)
        {
            var num = number * 0.001;
            str = $"{string.Format("{0:N" + dec + "}", num)}";
            end = "千";
        }
        else
        {
            str = number.ToString();
        }
        if (dec > 0 && str.IndexOf(".") != -1)
        {
            var length = str.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                var subChar = str.Substring(i, 1);
                if (subChar == ".")
                {
                    str = str.Substring(0, i);
                    break;
                }
                if (subChar == "0")
                {
                    str = str.Substring(0, i);
                }
                else
                    break;
            }
        }

        return str + end;
    }

    public static Transform PhysicsHit(Vector3 pos, float maxDistance, int layerMask)
    {
        var ray = GameController.Instance.MainCamera.ScreenPointToRay(pos);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit, maxDistance, layerMask) ? hit.transform : null;
    }

    public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask)
    {
        return Physics.Raycast(ray, out hitInfo, maxDistance, layerMask);
    }

    /// <summary>
    /// 数字格式化
    /// </summary>
    /// <param name="number">原数字</param><param name="dec">返回的字符串小数精度</param>
    /// 无单位(1:10000)
    public static string NumberFormatHaveNotUnit(long number, int dec = 2)
    {
        var str = string.Empty;
        double num = number * 0.0001;
        str = num.ToString();

        if (dec > 0 && str.IndexOf(".") != -1)
        {
            if (str.Length > (str.IndexOf(".") + dec + 1))
            {
                str = str.Substring(0, str.IndexOf(".") + dec + 1);
            }
        }
        return str;
    }


    /// <summary>
    /// 小数点保留两位数
    /// </summary>
    public static string GetPreciseDecimal(float num, int dec = 2)
    {
        var str = num.ToString();

        if (dec > 0 && str.IndexOf(".") != -1)
        {
            if (str.Length > (str.IndexOf(".") + dec + 1))
            {
                str = str.Substring(0, str.IndexOf(".") + dec + 1);
            }
        }
        return str;
    }

    //小游戏读取大厅配置表
    public static string LoadConfigByTableName(string fileName) {
        return TableLoadHelper.LoadConfigByTableName(fileName);
    }
}