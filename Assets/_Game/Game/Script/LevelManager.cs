using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private IWaveManager _waveManager;
    private TowerController _towerController;
    public void Init(IWaveManager waveManager, TowerController towerController)
    {
        _waveManager = waveManager;
        _towerController = towerController;

        _waveManager.OnKilledAllWave += SuccessEnd;
        _towerController.OnTowerDie += FailEnd;
    }

    private void OnDestroy()
    {
        _waveManager.OnKilledAllWave -= SuccessEnd;
        _towerController.OnTowerDie -= FailEnd;
    }

    public void StartLevel()
    {
        _towerController.StartLevel();
        _waveManager.StartSpawn();
    }

    private void SuccessEnd() => EndLevel(true);
    private void FailEnd() => EndLevel(false);
    private void EndLevel(bool success)
    {
        Invoke(nameof(ShowEndgamePopup), 2f);
    }

    void ShowEndgamePopup()
    {
        _waveManager.KillAll();
        PopupManager.Instance.ShowImmediately<PopupEndgame>(new PopupEndgame.Entry(_waveManager, this));
    }

    public void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
