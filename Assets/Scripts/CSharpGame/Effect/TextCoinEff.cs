using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextCoinEff : MonoBehaviour
{
    private Transform thisT;           //本体Transform
    private Text uiText;               //Text 控件
    private float scaleMax = 0;        //缩放-最大
    private Color orgColor;            //恢复原先颜色
    private bool isScale1 = false;     //一阶段缩放标记
    private bool isScale2 = false;     //二阶段缩放标记
    private bool isMove = false;       //移动标记
    private float Scale = 0;           //缩放-最小
    private string NumberStr;          //显示的数字

    [Header("最大比例")]
    public float scaleMaxRat = 1.4f;
    [Header("开始停留时间")]
    public float stayTime = 0.4f;
    [Header("上移速度")]
    public float moveSpeed = 1;
    [Header("淡出时间")]
    public float fadeTime = 0.8f;
    [Header("缩放速度")]
    public float scaleSpeed = 0.8f;
    [Header("淡出速度")]
    public float fadeSpeed = 1;

    private void Awake()
    {
        thisT = transform;
        uiText = GetComponent<Text>();
        orgColor = uiText.color;
    }

    public void Play(float scale, string numStr)
    {
        NumberStr = numStr;
        Scale = scale;
        thisT.localScale = Vector3.one * Scale;
        isScale1 = true;
        isScale2 = false;
        uiText.color = orgColor;
        scaleMax = Scale * scaleMaxRat;
        uiText.text = $"+{NumberStr}";
        StopCoroutine("move");
        StartCoroutine("move");
    }

    private void Update()
    {
        if (isScale1)
        {
            thisT.localScale += (Vector3.one * scaleSpeed * Time.deltaTime);
            if (thisT.localScale.x >= scaleMax)
            {
                isScale2 = true;
                isScale1 = false;
                thisT.localScale = Vector3.one * scaleMax;
            }
        }
        if (isScale2)
        {
            thisT.localScale -= (Vector3.one * scaleSpeed * Time.deltaTime);
            if (thisT.localScale.x <= Scale)
            {
                isScale2 = false;
                thisT.localScale = Vector3.one * Scale;
            }
        }

        if (isMove)
        {
            thisT.position += Vector3.up * moveSpeed * Time.deltaTime;
            uiText.color -= new Color(0, 0, 0, fadeSpeed * Time.deltaTime);
        }
    }

    private IEnumerator move()
    {
        isMove = false;
        yield return new WaitForSeconds(stayTime);
        isMove = true;
        yield return new WaitForSeconds(fadeTime);
        ObjectPoolManager.Instance.Unspawn(gameObject);
    }

}
