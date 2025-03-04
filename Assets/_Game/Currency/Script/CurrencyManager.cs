using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICurrencyManager
{
    Dictionary<CurrencyType, int> CurrencyValue { get; }
    event Action<CurrencyType, int> OnCurrencyChanged;
    void AddCurrency(CurrencyType currencyType, int amount);
    bool IsEnough(CurrencyType currencyType, int amount);
}

public enum CurrencyType
{
    Silver, Gold
}
public class CurrencyManager : MonoBehaviour, ICurrencyManager
{
    public Dictionary<CurrencyType, int> CurrencyValue { get; } = new();
    public event Action<CurrencyType, int> OnCurrencyChanged;
    public void AddCurrency(CurrencyType currencyType, int amount)
    {
        CurrencyValue[currencyType] += amount;
        PlayerPrefs.SetInt(currencyType.ToString(), CurrencyValue[currencyType]);
        OnCurrencyChanged?.Invoke(currencyType, CurrencyValue[currencyType]);
    }
    public bool IsEnough(CurrencyType currencyType, int amount) => CurrencyValue[currencyType] >= amount;

    public void Init()
    {
        for (int i = 0; i < Enum.GetValues(typeof(CurrencyType)).Length; i++)
        {
            if (i == (int)CurrencyType.Silver) CurrencyValue[(CurrencyType)i] = 0;
            else CurrencyValue[(CurrencyType)i] = PlayerPrefs.GetInt($"{(CurrencyType)i}", 0);
        }
    }
}
