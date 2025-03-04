using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIUpgrade uiUpgrade;

    public void Init(IStatManager statManager, ICurrencyManager currencyManager)
    {
        uiUpgrade.Init(statManager, currencyManager);
    }
}
