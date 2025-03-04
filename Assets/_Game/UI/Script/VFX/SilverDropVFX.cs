using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SilverDropVFX : Poolable
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text silverTxt;

    public void Show(int value, Vector3 position)
    {
        transform.position = position;
        silverTxt.text = $"+{value}";
        
        Sequence sequence = DOTween.Sequence().SetUpdate(true);
        sequence.Append(canvasGroup.DOFade(1f, 0.3f).From(0f))
            .Join(transform.DOScale(1, 0.3f).From(0).SetEase(Ease.OutQuad))
            .Append(canvasGroup.DOFade(0f, 0.5f))
            .Join(transform.DOLocalMoveY(30f, 0.5f).SetRelative())
            .OnComplete(ReturnToPool);
    }
    
    void ReturnToPool() => PoolManager.Instance.ReturnObject(this);
}
