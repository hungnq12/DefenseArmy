using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    private Dictionary<GameObject, IObjectPool> _pools = new Dictionary<GameObject, IObjectPool>();
    [SerializeField] private Transform uiPoolContainer;

    private void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
        }
    }

    public void CreatePool<T>(T prefab, int size) where T : MonoBehaviour 
    {
        GameObject prefabGO = prefab.gameObject;
        if (!_pools.ContainsKey(prefabGO)) 
        {
            _pools[prefabGO] = new ObjectPool<T>(prefab, size);
        }
    }

    public T GetObject<T>(T prefab, Transform parentOverride = null, bool isUI = false) where T : MonoBehaviour 
    {
        GameObject prefabGO = prefab.gameObject;
        if (!_pools.ContainsKey(prefabGO)) 
        {
            CreatePool(prefab, 0);
        }
        IObjectPool pool = _pools[prefabGO];
        if (pool is IObjectPool<T> typedPool)
            return typedPool.GetObject(parentOverride != null ? parentOverride : !isUI ? transform : uiPoolContainer);
        Debug.LogWarning($"Pool exists but cannot be cast to pool of type {typeof(T)}");
        return null;
    }

    public void ReturnObject<T>(T obj) where T : MonoBehaviour 
    {
        if (obj is Poolable poolable)
        {
            GameObject key = poolable.PoolKey;
            if (_pools.TryGetValue(key, out IObjectPool pool)) 
            {
                if (pool is IObjectPool<T> typedPool)
                {
                    typedPool.ReturnObject(obj);
                    return;
                }
            }
            Debug.LogWarning($"No Pool found for key {key}");
        }
        else
        {
            Debug.LogWarning("ReturnObject: Object does not implement IPoolable");
        }
    }
}
