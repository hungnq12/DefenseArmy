using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]
public abstract class Popup : MonoBehaviour
{
    protected Image _transBG;
    protected RectTransform _popupRect;
    protected CanvasGroup _popupCanvasGroup;
    Sequence _showSequence;
    Sequence _hideSequence;
    object _data;
    float _transBgAlpha;

    public object Data => _data;
    public PopupShowMode ShowMode = PopupShowMode.ScaleUp;
    public const float ANIM_TIME = 0.3f;
    public event Action OnShowCompleteAction;
    public event Action OnHideCompleteAction;
    public event Action OnShowStartAction;
    public event Action OnHideStartAction;

    private void Awake()
    {
        _transBG = GetComponent<Image>();
        _popupRect = transform.GetChild(0).GetComponent<RectTransform>();
        _popupCanvasGroup = GetComponent<CanvasGroup>();
        _transBgAlpha = _transBG.color.a;
    }

    public virtual void InitPopup(object data)
    {
        _data = data;
        _popupCanvasGroup.interactable = false;
    }

    void ShowBG()
    {
        _transBG.gameObject.SetActive(true);
        _transBG.DOFade(_transBgAlpha, ANIM_TIME).From(0f).SetUpdate(true);
    }

    public virtual void ShowPopup()
    {
        //AudioManager.Current.PlaySFX(1, audioName:"infor_show");
        OnShowStartAction?.Invoke();
        transform.SetAsLastSibling();
        gameObject.SetActive(true);
        ShowBG();
        _showSequence = DOTween.Sequence();
        switch (ShowMode)
        {
            case PopupShowMode.ScaleUp:
                _showSequence.Join(_popupRect.DOScale(1f, ANIM_TIME).From(0f));
                break;
            case PopupShowMode.FlyRight:
                _showSequence.Join(_popupRect.DOLocalMoveX(0, ANIM_TIME).From(-_popupRect.rect.width));
                break;
            case PopupShowMode.FlyLeft:
                _showSequence.Join(_popupRect.DOLocalMoveX(0, ANIM_TIME).From(_popupRect.rect.width));
                break;
            case PopupShowMode.BlackOut:
                //_showSequence.Join(_popupRect.DOLocalMoveY(10, ANIM_TIME / 2).From(-20));
                _showSequence.Join(_popupCanvasGroup.DOFade(1, ANIM_TIME / 2).From(0f));
                //_showSequence.Append(_popupRect.DOLocalMoveY(0, ANIM_TIME / 2));
                break;
            case PopupShowMode.FlyBot:
                _showSequence.Join(_popupRect.DOLocalMoveY(0, ANIM_TIME).From(-_popupRect.rect.height));
                break;
            case PopupShowMode.FlyTop:
                _showSequence.Join(_popupRect.DOLocalMoveY(0, ANIM_TIME).From(_popupRect.rect.height));
                break;
            case PopupShowMode.None:
                _popupRect.localScale = Vector3.one;
                _popupRect.localPosition = Vector3.zero;
                break;
        }

        if (_showSequence != null)
            _showSequence.SetUpdate(true).OnComplete(OnShowComplete);
        else OnShowComplete();
        LoadData();
    }

    protected virtual void OnShowComplete()
    {
        OnShowCompleteAction?.Invoke();
        _popupCanvasGroup.interactable = true;
    }

    public virtual void HidePopup()
    {
        //AudioManager.Current.PlaySFX(1, audioName:"infor_hide");
        OnHideStartAction?.Invoke();
        _popupCanvasGroup.interactable = false;
        HideBG();
        _hideSequence = DOTween.Sequence();
        switch (ShowMode)
        {
            case PopupShowMode.ScaleUp:
                _hideSequence.Join(_popupRect.DOScale(0f, ANIM_TIME).From(1f));
                break;
            case PopupShowMode.FlyRight:
                _hideSequence.Join(_popupRect.DOLocalMoveX(-_popupRect.rect.width, ANIM_TIME).From(0));
                break;
            case PopupShowMode.FlyLeft:
                _hideSequence.Join(_popupRect.DOLocalMoveX(_popupRect.rect.width, ANIM_TIME).From(0));
                break;
            case PopupShowMode.BlackOut:
                //_hideSequence.Prepend(_popupRect.DOLocalMoveY(10, ANIM_TIME / 2));
                //_hideSequence.Append(_popupRect.DOLocalMoveY(-20, ANIM_TIME / 2));
                _hideSequence.Join(_popupCanvasGroup.DOFade(0, ANIM_TIME / 2));
                break;
            case PopupShowMode.FlyBot:
                _hideSequence.Join(_popupRect.DOLocalMoveY(-_popupRect.rect.height, ANIM_TIME).From(0));
                break;
            case PopupShowMode.FlyTop:
                _hideSequence.Join(_popupRect.DOLocalMoveY(_popupRect.rect.height, ANIM_TIME).From(0));
                break;
            case PopupShowMode.None:
                break;
        }

        if (_hideSequence != null)
            _hideSequence.SetUpdate(true).OnComplete(OnHideComplete);
        else
            OnHideComplete();
    }

    public virtual void HideDestroyPopup<T>()
    {
        HidePopup();
        PopupManager.Instance.InvokeCloseAndDestroyPopup<T>(this);
    }

    void HideBG()
    {
        if (_transBG != null)
        {
            _transBG.gameObject.SetActive(true);
            _transBG.DOFade(0, ANIM_TIME / 2f).From(_transBgAlpha).SetUpdate(true);
        }
    }

    protected virtual void OnHideComplete()
    {
        gameObject.SetActive(false);
        OnHideCompleteAction?.Invoke();
    }

    protected abstract void LoadData();
}

public class PopupQueueConfig
{
    public Popup Popup;
    public object Data;
}

public enum PopupShowMode
{
    None,
    ScaleUp,
    FlyRight,
    FlyLeft,
    BlackOut,
    FlyBot,
    FlyTop
}