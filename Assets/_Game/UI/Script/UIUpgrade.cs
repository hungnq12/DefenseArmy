using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpgrade : MonoBehaviour
{
    [SerializeField] private UIUpgradeStatItem statItemPrefab;
    [SerializeField] private Transform statItemParent;
    [SerializeField] private List<StatType> tab1;
    private List<UIUpgradeStatItem> statItems = new List<UIUpgradeStatItem>();
    private IStatManager _statManager;
    private ICurrencyManager _currencyManager;

    public void Init(IStatManager statManager, ICurrencyManager currencyManager)
    {
        _statManager = statManager;
        _currencyManager = currencyManager;

        OnChangeTabStat(0);
    }
    
    public void OnChangeTabStat(int tabID)
    {
        foreach (var statItem in statItems) PoolManager.Instance.ReturnObject(statItem);
        statItems.Clear();
        
        foreach (var type in tab1)
        { 
            var statItem = PoolManager.Instance.GetObject(statItemPrefab, statItemParent);
            statItem.Init(_statManager, type, _currencyManager);
            statItems.Add(statItem);
        }
    }
}
