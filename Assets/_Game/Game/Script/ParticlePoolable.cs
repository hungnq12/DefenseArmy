using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolable : Poolable
{
    public void Show(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(rotation);
        transform.localScale = scale;
        
        Invoke(nameof(ReturnToPool), 2f);
    }
    
    void ReturnToPool() => PoolManager.Instance.ReturnObject(this);
}
