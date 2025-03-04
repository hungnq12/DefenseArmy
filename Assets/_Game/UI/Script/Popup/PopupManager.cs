using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }
    [SerializeField] private PopupHolder popupHolder;
    private Queue<PopupQueueConfig> _popupQueue;
    private readonly Dictionary<Type, Popup> _popupInstance = new();
    private PopupQueueConfig _currentPopupQueueConfig;

    private void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
        }
    }

    private Popup InstantiatePopup<T>()
    {
        if (popupHolder.GetPopup<T>(out Popup value))
        {
            if (value != null)
            {
                if (!_popupInstance.ContainsKey(typeof(T)))
                {
                    //Spawn GameObject
                    Popup pElement = Instantiate(value, transform);
                    pElement.transform.localScale = Vector3.one;
                    pElement.gameObject.SetActive(false);
                    //Save to dic
                    _popupInstance[typeof(T)] = pElement;
                    return pElement;
                }
            }
            else
            {
                Debug.Log("Popup is not existed");
                return null;
            }
        }
        return _popupInstance[typeof(T)];
    }

    public object ShowPopupInQueue<T>(object data = null)
    {
        var popupGO = InstantiatePopup<T>();
        if (popupGO != null)
        {
            _popupQueue.Enqueue(new PopupQueueConfig() { Popup = popupGO, Data = data });
            if (_currentPopupQueueConfig == null)
            {
                ShowNextPopupInQueue();
            }
        }
        return popupGO;
    }

    public object ShowImmediately<T>(object data = null)
    {
        var popupGO = InstantiatePopup<T>();
        if (popupGO != null)
        {
            popupGO.InitPopup(data); 
            popupGO.ShowPopup();
            /*PauseQueue();
            popupGO.OnHideCompleteAction -= ContinueQueue;
            popupGO.OnHideCompleteAction += ContinueQueue;*/
        }
        return popupGO;

    }
    private void ShowNextPopupInQueue()
    {
        //if (!_canShowQueue) return;
        if (_popupQueue.Count > 0)
        {
            Debug.Log($"Popup Queue Count: {_popupQueue.Count}");
            _currentPopupQueueConfig = _popupQueue.Dequeue();
            if (_currentPopupQueueConfig != null)
            {
                _currentPopupQueueConfig.Popup.InitPopup(_currentPopupQueueConfig.Data);
                _currentPopupQueueConfig.Popup.ShowPopup();
                _currentPopupQueueConfig.Popup.OnHideCompleteAction -= OnClosePopupInQueue;
                _currentPopupQueueConfig.Popup.OnHideCompleteAction += OnClosePopupInQueue;
                
                Debug.Log($"Popup Show: {_currentPopupQueueConfig.Popup.name}");
            }
        }
        else
        {
            _currentPopupQueueConfig = null;
        }
    }
    
    private void OnClosePopupInQueue()
    {
        _currentPopupQueueConfig = null;
        ShowNextPopupInQueue();
    }

    public void InvokeCloseAndDestroyPopup<T>(Popup popup)
    {
        if (_popupInstance.ContainsKey(popup.GetType()))
        {
            //Destroy GameObject
            Destroy(_popupInstance[popup.GetType()].gameObject);
            _popupInstance.Remove(popup.GetType());
        }
    }
    
    /*private bool _canShowQueue = true;
    public void PauseQueue()
    {
        _canShowQueue = false;
    }

    public void ContinueQueue()
    {
        _canShowQueue = true;
        if (_currentPopupQueueConfig == null)
        {
            ShowNextPopupInQueue();
        }
    }*/
}
