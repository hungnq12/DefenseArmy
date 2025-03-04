using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    private EnemyController _enemyController;
    private Transform _target;
    public void Init(EnemyController enemyController, Transform target)
    {
        _enemyController = enemyController;
        _target = target;
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _enemyController.OnStateChanged += MoveToTarget;
        _enemyController.OnEnemyDie += UnsubscribeEvents;
    }

    private void UnsubscribeEvents(EnemyController _)
    {
        _enemyController.OnStateChanged -= MoveToTarget;
        _enemyController.OnEnemyDie -= UnsubscribeEvents;
    }

    private void MoveToTarget(EnemyState state)
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
