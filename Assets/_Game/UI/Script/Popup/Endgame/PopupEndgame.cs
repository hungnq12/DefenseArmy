using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupEndgame : Popup
{
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private TMP_Text waveTxt;
    private IWaveManager _waveManager;
    private LevelManager _levelManager;
    protected override void LoadData()
    {
        Entry entry = Data as Entry;
        _waveManager = entry.WaveManager;
        _levelManager = entry.LevelManager;
        levelTxt.text = $"Level {_waveManager.CurrentLevelID + 1}";
        waveTxt.text = $"Turn {_waveManager.CurrentWaveID + 1}";
    }

    public void OnClickContinue()
    {
        HidePopup();
    }

    protected override void OnHideComplete()
    {
        base.OnHideComplete();
        _levelManager.ReloadLevel();
    }

    public class Entry
    {
        public readonly IWaveManager WaveManager;
        public readonly LevelManager LevelManager;

        public Entry(IWaveManager waveManager, LevelManager levelManager)
        {
            WaveManager = waveManager;
            LevelManager = levelManager;
        }
    }
}
