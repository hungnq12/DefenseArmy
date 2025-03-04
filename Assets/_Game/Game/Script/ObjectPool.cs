using System.Collections.Generic;
using UnityEngine;

public abstract class IPoolable : MonoBehaviour
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
    T GetObject();
    void ReturnObject(T obj);
}

public class ObjectPool<T> : IObjectPool<T> where T : MonoBehaviour 
{
    private Queue<T> _pool = new Queue<T>();
    private T _prefab;
    private Transform _parent;

    public ObjectPool(T prefab, int size, Transform parent) 
    {
        _prefab = prefab;
        _parent = parent;
        for (int i = 0; i < size; i++) 
        {
            T obj = GameObject.Instantiate(prefab, parent);
            if (obj is IPoolable poolable)
            {
                poolable.InitializePoolKey(prefab.gameObject);
            }
            ReturnObject(obj);
        }
    }

    public T GetObject() 
    {
        T obj = _pool.Count > 0 ? _pool.Dequeue() : GameObject.Instantiate(_prefab, _parent);
        if (obj is IPoolable poolable)
        {
            poolable.InitializePoolKey(_prefab.gameObject);
        }
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnObject(T obj) 
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_parent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        _pool.Enqueue(obj);
    }
}
