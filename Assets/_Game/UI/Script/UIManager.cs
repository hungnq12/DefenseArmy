using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UICurrency uiCurrency;
    [SerializeField] private UIUpgrade uiUpgrade;
    [SerializeField] private UILevel uiLevel;
    [SerializeField] private UITower uiTower;

    public void Init(IStatManager statManager, ICurrencyManager currencyManager, IWaveManager waveManager, TowerController towerController)
    {
        uiCurrency.Init(currencyManager);
        uiUpgrade.Init(statManager, currencyManager);
        uiLevel.Init(waveManager);
        uiTower.Init(towerController);
    }
}
