
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private StatManager statManager;
    [SerializeField] private TowerController towerController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CameraManager cameraManager;

    private void Start()
    {
        Application.targetFrameRate = 60;
        currencyManager.Init();
        statManager.Init();
        towerController.Init(statManager);
        waveManager.Init(currencyManager, towerController.transform);
        levelManager.Init(waveManager, towerController, cameraManager);
        uiManager.Init(statManager, currencyManager, waveManager, towerController);
        
        levelManager.StartLevel();
    }
}
