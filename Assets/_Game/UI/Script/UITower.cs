using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITower : MonoBehaviour
{
    [SerializeField] private TMP_Text towerHPTxt;
    private TowerController _towerController;

    public void Init(TowerController towerController)
    {
        _towerController = towerController;
        _towerController.OnHPChanged += UpdateTowerHP;
    }

    private void OnDestroy()
    {
        _towerController.OnHPChanged -= UpdateTowerHP;
    }

    private void UpdateTowerHP(float currentHP, float maxHP)
    {
        towerHPTxt.text = $"{currentHP} / {maxHP}";
    }
}
