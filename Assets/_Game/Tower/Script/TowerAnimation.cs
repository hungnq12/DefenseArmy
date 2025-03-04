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

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _towerController.OnTowerAttack += PlayAttackAnimation;
    }

    private void UnsubscribeEvents()
    {
        _towerController.OnTowerAttack -= PlayAttackAnimation;
    }

    private void PlayAttackAnimation(float _)
    {
        animator.Play(TowerState.Attack.ToString());
    }
}
