using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[LuaCallCSharp]
public class AnimPlayer : MonoBehaviour 
{
    private Sprite start;
    public Sprite[] Sprites; //精灵数组
    public int Frame = 10;     //每秒几帧
    public bool Loop = false;  //是否循环
    public bool HideWhenDone = true;
    public bool PlayAwake = true;
    public bool ShowFirst = true;  //动画结束后显示初始图片false 显示最后一张
    private System.Action callback;
    private Image imgComponent;
    private SpriteRenderer spriteRender;
    private bool isPlay = false;
    private bool isPause = false;
    private WaitForSeconds wait;

    void Awake()
    {
        imgComponent = GetComponent<Image>();
        spriteRender = GetComponent<SpriteRenderer>();
        if (imgComponent && spriteRender)
            Debug.LogError("存在冲突组件：<SpriteRenderer> 与 <Image>");
        if (imgComponent)
            start = imgComponent.sprite;
        else
            start = spriteRender.sprite;
    }

    void OnEnable()
    {
        if (imgComponent)
            imgComponent.sprite = start;
        else
            spriteRender.sprite = start;
        isPlay = false;
        isPause = false;
        if (PlayAwake)
            Play();
    }

    public void Play(System.Action call = null)
    {
        StopCoroutine("Play_Cor");
        StartCoroutine("Play_Cor");
        callback = call;
    }

    public bool IsPlay()
    {
        return isPlay;
    }

    public void SetFrame(int frame)
    {
        Frame = frame;
        float time = 1f / Mathf.Max(1, Frame);
        wait = new WaitForSeconds(time);
    }

    public void Pause()
    {
        isPause = true;
        wait = new WaitForSeconds(1000000);
    }

    public void UnPause()
    {
        isPause = false;
        float time = 1f / Mathf.Max(1, Frame);
        wait = new WaitForSeconds(time);
    }

    public bool IsPause()
    {
        return isPause;
    }

    IEnumerator Play_Cor()
    {
        float time = 1f / Mathf.Max(1, Frame);
        wait = new WaitForSeconds(time);
        if (Sprites.Length == 0)
            yield break;
        isPlay = true;
        do 
        {
            for (int i = 0; i < Sprites.Length; i++)
            {
                if (Sprites[i] == null)
                {
                    yield return wait;
                }
                else
                {
                    if (imgComponent)
                        imgComponent.sprite = Sprites[i];
                    else
                        spriteRender.sprite = Sprites[i];
                    yield return wait;
                }
            }
        } while (Loop);

        isPlay = false;
        yield return wait;
        if (callback != null)
            callback();
        if (ShowFirst)
        {
            if (imgComponent)
                imgComponent.sprite = start;
            else
                spriteRender.sprite = start;
        }
        gameObject.SetActive(!HideWhenDone);
    }
}
