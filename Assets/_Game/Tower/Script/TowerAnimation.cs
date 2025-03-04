using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private TowerController _towerController;
    public void Init(TowerController towerController)
    {
        _towerController = towerController;
        SubscribeEvents();
    }
    
    private void SubscribeEvents()
    {
        _towerController.OnStateChanged += PlayAnimation;
        _towerController.OnTowerDie += UnsubscribeEvents;
    }

    private void UnsubscribeEvents()
    {
        _towerController.OnStateChanged -= PlayAnimation;
        _towerController.OnTowerDie -= UnsubscribeEvents;
    }

    private void PlayAnimation(TowerState state)
    {
        animator.Play(state.ToString());
    }
}
