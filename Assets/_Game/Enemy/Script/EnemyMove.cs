using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    private EnemyController _enemyController;
    private Transform _target;
    public void Init(EnemyController enemyController, Transform target, float moveSpeed)
    {
        _enemyController = enemyController;
        _target = target;
        agent.speed = moveSpeed;
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _enemyController.OnStateChanged += HandleChangeState;
        _enemyController.OnEnemyDie += UnsubscribeEvents;
    }

    private void UnsubscribeEvents(EnemyController _)
    {
        _enemyController.OnStateChanged -= HandleChangeState;
        _enemyController.OnEnemyDie -= UnsubscribeEvents;
    }

    private void HandleChangeState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Move:
                agent.isStopped = false;
                agent.SetDestination(_target.position);
                break;
            default:
                agent.isStopped = true;
                break;
        }
    }
}
