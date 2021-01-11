using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinEff : MonoBehaviour
{
    private Transform thisT;
    private float jumpHeight = 0.6f;
    private float height;
    private float jumpSpeed = 3;
    private float moveSpeed;
    private int count;  //跳跃次数
    private bool upTag;
    private bool downTag;
    private float stayTime = 0.5f;  //停留时间
    private bool isStay = true;
    private bool isDone = false;
    private Vector3 moveToPos;

    [Header("跳跃的次数")]
    public int jumpCount = 2;
    [Header("开始速度")]
    public float startSpeed = 2;
    [Header("最小速度")]
    public float minSpeed = 1;
    [Header("最大速度")]
    public float maxSpeed = 10;
    [Header("加速")]
    public float acc = 1;
    [Header("动画结束后是否隐藏")]
    public bool HideWhenDone = true;
    
    private void Awake()
    {
        thisT = transform;
    }
    private Action<bool> action;
    private bool isLast;

    public void Play(Vector3 pos, Action<bool> action,bool isLast, float stayTime = 2)
    {
        this.isLast = isLast;
        this.action = action;
        isDone = false;
        moveToPos = pos;
        height = 0;
        count = 0;
        upTag = true;
        downTag = false;
        moveSpeed = startSpeed;
        this.stayTime = stayTime;
        StartCoroutine(coinStay());
    }

    private void Update()
    {
        if (isDone)
            return;
        if (count < jumpCount)
        {
            Vector3 h = Vector3.up * jumpSpeed * Time.deltaTime;
            if (upTag)
            {
                thisT.position += h;
                height += h.y;
                if (height > jumpHeight / (count + 1))
                {
                    upTag = false;
                    downTag = true;
                }
            }
            if (downTag)
            {
                thisT.position -= h;
                height -= h.y;
                if (height < 0)
                {
                    height = 0;
                    count++;
                    upTag = true;
                    downTag = false;
                }
            }
        }
        else if (isStay == false)
        {
            moveSpeed += acc;   //这里搞个加速效果
            moveSpeed = Mathf.Clamp(moveSpeed, minSpeed, maxSpeed);  //速度至少为1，不能降为0或者负数
            Vector3 pos = (moveToPos - thisT.position).normalized * moveSpeed * Time.deltaTime;
            thisT.position += pos;
            float dis = Vector2.Distance(thisT.position, moveToPos);

            if (dis < 0.001)
            {
                action?.Invoke(isLast);
                AudioManager.Instance.PlaySoundEff2D(SysDefines.AUDIO + "jinbihuishou");
                isDone = true;
                if (HideWhenDone)
                    ObjectPoolManager.Instance.Unspawn(thisT.gameObject);
            }
            if (dis < moveSpeed * Time.deltaTime)
                thisT.position = moveToPos;
        }
    }

    private IEnumerator coinStay()
    {
        isStay = true;
        yield return new WaitForSeconds(stayTime);
        isStay = false;
    }
}
