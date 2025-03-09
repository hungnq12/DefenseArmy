using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private IWaveManager _waveManager;
    private TowerController _towerController;
    private CameraManager _cameraManager;
    public void Init(IWaveManager waveManager, TowerController towerController, CameraManager cameraManager)
    {
        _waveManager = waveManager;
        _towerController = towerController;
        _cameraManager = cameraManager;

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
        _cameraManager.StartLevel();
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
