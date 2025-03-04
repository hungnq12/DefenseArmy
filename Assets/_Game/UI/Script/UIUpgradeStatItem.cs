using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeStatItem : Poolable
{
    [SerializeField] private Sprite[] upgradeBgSprites;
    [SerializeField] private TMP_Text statNameTxt;
    [SerializeField] private TMP_Text currentValueTxt;
    [SerializeField] private TMP_Text nextValueTxt;
    [SerializeField] private TMP_Text priceTxt;
    [SerializeField] private GameObject maxLevel;
    [SerializeField] private Image upgradeBg;
    private IStatManager _statManager;
    private ICurrencyManager _currencyManager;
    private StatType _statType;
    private int _price;
    private List<StatConfig> StatConfigs => _statManager.TowerData.StatConfigs(_statType);
    private int CurrentLevel => _statManager.StatLevelIngame[_statType];

    public void Init(IStatManager statManager, StatType statType, ICurrencyManager currencyManager)
    {
        _statManager = statManager;
        _statType = statType;
        _currencyManager = currencyManager;
        _currencyManager.OnCurrencyChanged += UpdateButtonBg;
        statNameTxt.text = _statType.ToString();
        UpdateUI();
    }

    private void OnDestroy()
    {
        _currencyManager.OnCurrencyChanged -= UpdateButtonBg;
    }

    private void UpdateButtonBg(CurrencyType type, int value)
    {
        if (type == CurrencyType.Silver)
        {
            upgradeBg.sprite = value >= _price
                ? upgradeBgSprites[0]
                : upgradeBgSprites[1];
        }
    }

    private void UpdateUI()
    {
        if (CurrentLevel >= StatConfigs.Count - 1)
        {
            maxLevel.SetActive(true);
            return;
        }
        _price = StatConfigs[CurrentLevel - _statManager.StatLevel[_statType]].price;
        maxLevel.SetActive(false);
        currentValueTxt.text = StatConfigs[CurrentLevel].value.ToString();
        nextValueTxt.text = StatConfigs[CurrentLevel + 1].value.ToString();
        priceTxt.text = _price.ToString();
    }

    public void OnClickUpgrade()
    {
        if (CurrentLevel >= StatConfigs.Count - 1 || !_currencyManager.IsEnough(CurrencyType.Silver, _price))
        {
            return;
        }

        _currencyManager.AddCurrency(CurrencyType.Silver, -_price);
        _statManager.UpgradeStat(_statType);
        UpdateUI();
    }
}
