
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private StatManager statManager;
    [SerializeField] private TowerController towerController;
    [SerializeField] private UIManager uiManager;

    private void Start()
    {
        currencyManager.Init();
        statManager.Init();
        towerController.Init(statManager);
        waveManager.Init(currencyManager, towerController.transform);
        uiManager.Init(statManager, currencyManager);
        
        waveManager.StartWave();
    }
}
