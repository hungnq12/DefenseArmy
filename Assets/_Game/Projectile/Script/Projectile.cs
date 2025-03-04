using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Poolable
{
    [SerializeField] private float speed;
    private float _damage;
    private Vector3 _forwardDirection;
    
    public void Shoot(float damage, Vector3 spawnPos, Vector3 forwardDirection)
    {
        _damage = damage;
        _forwardDirection = forwardDirection;
        transform.position = spawnPos;
        transform.rotation = Quaternion.LookRotation(_forwardDirection);
        
        Invoke(nameof(ReturnToPool), 2f);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime), Space.Self);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyController>();
        if (enemy != null) 
        {
            enemy.InvokeEnemyTakeDamage(_damage);
            ReturnToPool();
        }
    }
    
    void ReturnToPool() => PoolManager.Instance.ReturnObject(this);
}
