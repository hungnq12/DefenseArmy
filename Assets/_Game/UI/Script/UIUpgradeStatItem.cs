using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeStatItem : IPoolable
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
    private List<StatConfig> StatConfigs => _statManager.TowerData.StatConfigs(_statType);
    private int CurrentLevel => _statManager.StatLevelIngame[_statType];
    private int Price => StatConfigs[CurrentLevel - _statManager.StatLevel[_statType]].price;
    public void Init(IStatManager statManager, StatType statType, ICurrencyManager currencyManager)
    {
        _statManager = statManager;
        _currencyManager = currencyManager;
        _currencyManager.OnCurrencyChanged -= UpdateButtonBg;
        _currencyManager.OnCurrencyChanged += UpdateButtonBg;
        statNameTxt.text = statType.ToString();
        UpdateUI();
    }

    private void UpdateButtonBg(CurrencyType type)
    {
        if (type == CurrencyType.Gold)
            upgradeBg.sprite = _currencyManager.IsEnough(CurrencyType.Silver, Price) ? upgradeBgSprites[0] : upgradeBgSprites[1];
    }

    private void UpdateUI()
    {
        if (CurrentLevel >= StatConfigs.Count - 1)
        {
            maxLevel.SetActive(true);
            return;
        }
        maxLevel.SetActive(false);
        currentValueTxt.text = StatConfigs[CurrentLevel].value.ToString();
        nextValueTxt.text = StatConfigs[CurrentLevel + 1].value.ToString();
        priceTxt.text = Price.ToString();
    }

    public void OnClickUpgrade()
    {
        if (CurrentLevel >= StatConfigs.Count - 1 || !_currencyManager.IsEnough(CurrencyType.Silver, Price))
        {
            return;
        }
        _currencyManager.AddCurrency(CurrencyType.Silver, -Price);
        _statManager.UpgradeStat(_statType);
        UpdateUI();
    }
}
