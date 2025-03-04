using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Poolable : MonoBehaviour
{
    public GameObject PoolKey { get; private set; }
    public void InitializePoolKey(GameObject key)
    {
        PoolKey = key;
    }
}
public interface IObjectPool { }

public interface IObjectPool<T> : IObjectPool where T : MonoBehaviour 
{
    T GetObject(Transform parent);
    void ReturnObject(T obj);
}

public class ObjectPool<T> : IObjectPool<T> where T : MonoBehaviour 
{
    private Queue<T> _pool = new Queue<T>();
    private T _prefab;

    public ObjectPool(T prefab, int size) 
    {
        _prefab = prefab;
        for (int i = 0; i < size; i++)
        {
            GetObject();
        }
    }

    public T GetObject(Transform parent = null) 
    {
        T obj = _pool.Count > 0 ? _pool.Dequeue() : GameObject.Instantiate(_prefab, parent);
        if (obj is Poolable poolable)
        {
            poolable.InitializePoolKey(_prefab.gameObject);
        }
        obj.gameObject.SetActive(true);
        ResetTransform(obj);
        return obj;
    }

    public void ReturnObject(T obj) 
    {
        obj.gameObject.SetActive(false);
        ResetTransform(obj);
        _pool.Enqueue(obj);
    }

    private void ResetTransform(T obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
    }
}
