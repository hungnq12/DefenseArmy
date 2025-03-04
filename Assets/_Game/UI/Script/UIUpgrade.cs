using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgrade : MonoBehaviour
{
    [SerializeField] private UIUpgradeStatItem statItemPrefab;
    [SerializeField] private Transform statItemParent;
    private List<UIUpgradeStatItem> statItems = new List<UIUpgradeStatItem>();
    private StatType _currentStatType;
    private IStatManager _statManager;
    private ICurrencyManager _currencyManager;

    public void Init(IStatManager statManager, ICurrencyManager currencyManager)
    {
        _statManager = statManager;
        _currencyManager = currencyManager;

        OnChangeTabStat(0);
    }
    private void ShowStat(StatType statType)
    {
        foreach (var statItem in statItems) PoolManager.Instance.ReturnObject(statItem);
        statItems.Clear();
        
        switch (statType)
        {
            case StatType.Attack:
                for (int i = 0; i < Enum.GetValues(typeof(StatType)).Length; i++)
                { 
                    var statItem = PoolManager.Instance.GetObject(statItemPrefab, statItemParent);
                    statItem.Init(_statManager, (StatType)i, _currencyManager);
                    statItems.Add(statItem);
                }
                break;
        }
    }
    public void OnChangeTabStat(int statTypeID)
    {
        ShowStat((StatType)statTypeID);
    }
}
