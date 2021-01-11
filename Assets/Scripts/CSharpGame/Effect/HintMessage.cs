using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class HintMessage : MonoBehaviour
{
    #region Component组件
    private Text txtHintContent;
    private Text TxtHintContent
    {
        get
        {
            if (ReferenceEquals(txtHintContent, null))
                txtHintContent = UnityHelper.GetTheChildComponent<Text>(gameObject, "TipsFont");
            return txtHintContent;
        }
    }
    private Image imgBg;
    private Image ImgBg
    {
        get
        {
            if (ReferenceEquals(imgBg, null))
                imgBg = UnityHelper.GetTheChildComponent<Image>(gameObject, "BG");
            return imgBg;
        }
    }
    private CanvasGroup group;
    private CanvasGroup Group
    {
        get
        {
            if (ReferenceEquals(group, null))
                group = UnityHelper.GetTheChildComponent<CanvasGroup>(gameObject, "TipsFont");
            return group;
        }
    }
    #endregion

    //dotween 动画
    private Sequence quenece;
    private Action onCompleteCallback;

    #region life cycle function
    private void Awake()
    {
        quenece = DOTween.Sequence();
        quenece.Append(ImgBg.transform
                                .DOScale(Vector3.zero, 0.4f)
                                .From()
                                .SetEase(Ease.OutBack));
        quenece.Insert(0.25f, Group
                                .DOFade(0, 0.15f)
                                .From()
                                .SetEase(Ease.Linear));
        //quenece.AppendCallback(() =>
        //{
        //    if (!quenece.isBackwards)
        //    {
        //        transform
        //            .DOLocalMoveY(200f, 0.5f)
        //            .SetRelative(true)
        //            .SetDelay(0.3f)
        //            .SetEase(Ease.Linear);
        //    }
        //});
        quenece.AppendInterval(0.8f);
        quenece.OnComplete(() =>
        {
            quenece.PlayBackwards();
        });
        quenece.OnRewind(() =>
        {
            ObjectPoolManager.Instance.Unspawn(gameObject);
            if (!ReferenceEquals(onCompleteCallback, null))
            {
                onCompleteCallback();
            }
        });
        quenece.Pause();
        quenece.SetAutoKill(false);
    }

    private void OnEnable()
    {
        quenece.Restart();
    }
    #endregion

    #region public function
    public void SetHintContent(string content, Action completeCallback = null)
    {
        TxtHintContent.text = content;
        onCompleteCallback = completeCallback;
    }
    #endregion
}
