using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyController _enemyController;
    private float _atk;
    private float _atkSpd;
    private float _shootTimer;
    private bool _isInRange;

    public void Init(EnemyController enemyController)
    {
        _enemyController = enemyController;
    }
    
    private void Update()
    {
        Aim();
    }

    private void Aim()
    {
        if (!_isInRange) return;
        _shootTimer += Time.deltaTime * _atkSpd;
        if (_shootTimer >= 1f)
        {
            _shootTimer = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        //if (!_currentTarget.IsAlive) return;
        /*var bullet = bulletObj.Spawn(ShootPoint.position, null);
        Vector3 fireing = ShootPoint.forward;
        bullet.GetComponent<BulletTile>().Fire(fireing.normalized, _dame);
        muzle.Play();*/
    }
}
