using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICurrency : MonoBehaviour
{
    [SerializeField] private TMP_Text silverTxt;
    private ICurrencyManager _currencyManager;
    
    public void Init(ICurrencyManager currencyManager)
    {
        _currencyManager = currencyManager;
        _currencyManager.OnCurrencyChanged += UpdateUI;
        UpdateUI(CurrencyType.Silver, 0);
    }

    private void OnDestroy()
    {
        _currencyManager.OnCurrencyChanged -= UpdateUI;
    }

    private void UpdateUI(CurrencyType type, int value)
    {
        switch (type)
        {
            case CurrencyType.Silver:
                silverTxt.text = value.ToString();
                break;
        }
    }
}
