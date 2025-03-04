using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILevel : MonoBehaviour
{
    [SerializeField] private Image fillProgress;
    [SerializeField] private TMP_Text waveTxt;
    private IWaveManager  _waveManager;
    public void Init(IWaveManager waveManager)
    {
        _waveManager = waveManager;
        _waveManager.OnEnemyCountChanged += UpdateProgress;
        _waveManager.OnWaveChanged += UpdateWaveName;
    }

    private void OnDestroy()
    {
        _waveManager.OnEnemyCountChanged -= UpdateProgress;
        _waveManager.OnWaveChanged -= UpdateWaveName;
    }

    private void UpdateProgress(int killed, int total)
    {
        fillProgress.DOFillAmount((float)killed / total, 0.1f);
    }

    private void UpdateWaveName(int waveID)
    {
        waveTxt.text = $"Turn {waveID + 1}";
    }
}
