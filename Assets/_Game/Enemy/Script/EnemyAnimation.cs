using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private EnemyController _enemyController;

    public void Init(EnemyController enemyController)
    {
        _enemyController = enemyController;
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _enemyController.OnStateChanged += PlayAnimation;
        _enemyController.OnEnemyDie += UnsubscribeEvents;
    }

    private void UnsubscribeEvents(EnemyController _)
    {
        _enemyController.OnStateChanged -= PlayAnimation;
        _enemyController.OnEnemyDie -= UnsubscribeEvents;
    }
    
    private void PlayAnimation(EnemyState state)
    {
        animator.Play(state.ToString());
    }
}
